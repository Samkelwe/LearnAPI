using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LearnAPI.Models;
using LearnAPI.DTOs;
using System.Security.Cryptography;

namespace LearnAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BookContext _context;
        private readonly TokenService _tokenService;

        public UsersController(BookContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email already exists");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully", user.Id, user.Username, user.Email });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.Include(u => u.RefreshTokens)
                       .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return BadRequest("Invalid email or password");

            bool isCorrect = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isCorrect)
                return BadRequest("Invalid email or password");

            var accessToken = _tokenService.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            refreshToken.UserId = user.Id;
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                Email = user.Email,
                Username = user.Username
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            var refreshToken = await _context.RefreshTokens
               .Include(r => r.User)
               .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.IsExpired)
                return Unauthorized("Invalid or expired refresh token");

            // Rotate: revoke old
            refreshToken.IsRevoked = true;

            // Generate new pair
            var newAccessToken = _tokenService.GenerateToken(refreshToken.User);
            var newRefreshToken = GenerateRefreshToken();
            newRefreshToken.UserId = refreshToken.UserId;

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                Email = refreshToken.User.Email,
                Username = refreshToken.User.Username
            });
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }
    }
}