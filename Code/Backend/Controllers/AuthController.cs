using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto dto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isValid)
                return Unauthorized("Invalid credentials");

            var token = _jwtService.GenerateToken(user.UserId, user.Email);

            return Ok(new LoginResponseDto
            {
                Token = token,
                UserId = user.UserId,
                Email = user.Email
            });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password) ||
                string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("All fields are required");
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (existingUser != null)
                return BadRequest("User already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim().ToLower(),
                Password = hashedPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // ✅ auto-login after register (VERY IMPORTANT)
            var token = _jwtService.GenerateToken(user.UserId, user.Email);

            return Ok(new LoginResponseDto
            {
                Token = token,
                UserId = user.UserId,
                Email = user.Email
            });
        }



        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            // Do NOT reveal if email exists
            if (user == null)
                return Ok(new { message = "If the email exists, a reset link will be sent" });

            var token = Guid.NewGuid().ToString();

            var resetToken = new PasswordResetToken
            {
                UserId = user.UserId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false
            };

            _context.PasswordResetTokens.Add(resetToken);
            _context.SaveChanges();

            var resetLink = $"http://localhost:3000/reset-password?token={token}";
            Console.WriteLine($"RESET LINK: {resetLink}");

            return Ok(new { message = "If the email exists, a reset link will be sent" });
        }



        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordDto dto)
        {
            var resetToken = _context.PasswordResetTokens
                .FirstOrDefault(t =>
                    t.Token == dto.Token &&
                    !t.IsUsed &&
                    t.ExpiresAt > DateTime.UtcNow);

            if (resetToken == null)
                return BadRequest(new { message = "Invalid or expired token" });

            var user = _context.Users
                .FirstOrDefault(u => u.UserId == resetToken.UserId);

            if (user == null)
                return BadRequest(new { message = "Invalid token" });

            // Hash new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            // Invalidate token
            resetToken.IsUsed = true;

            _context.SaveChanges();

            return Ok(new { message = "Password reset successful" });
        }





    }
}
