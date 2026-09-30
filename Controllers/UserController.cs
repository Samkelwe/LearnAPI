using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LearnAPI.Models;
using LearnAPI.DTOs;

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

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("Email already exists");
            }

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

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return BadRequest("Invalid email or password");

            bool isCorrect = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isCorrect)
                return BadRequest("Invalid email or password");

            // Generate token
            var token = _tokenService.GenerateToken(user);

            return Ok(new
            {
                message = "Login successful",
                token = token,
                user.Id,
                user.Username,
                user.Email
            });
        }
    }
}