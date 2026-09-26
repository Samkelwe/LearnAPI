using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LearnAPI.Models;
using LearnAPI.DTOs;
using BCrypt.Net;

namespace LearnAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BookContext _context;

	public UsersController(BookContext context)
        {
            _context = context;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // 1. Check if user exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("Email already exists");
            }

            // 2. Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 3. Create user
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
    }
}