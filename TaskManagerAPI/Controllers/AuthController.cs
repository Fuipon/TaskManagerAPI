using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Data;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _auth;

        public AuthController(AppDbContext context, IAuthService auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            if (_context.Users.Any(u => u.UserName == request.Username))
                return BadRequest("Username taken");

            _auth.CreatePasswordHash(request.Password, out var hash, out var salt);

            var user = new User
            {
                UserName = request.Username,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (user == null || !_auth.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Invalid credentials");

            var token = _auth.CreateToken(user);
            return Ok(new { token });
        }
    }
    public class UserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
