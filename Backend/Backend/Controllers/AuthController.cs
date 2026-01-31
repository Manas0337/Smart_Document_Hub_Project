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

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            // IMPORTANT: Do NOT reveal if email exists
            if (user == null)
                return Ok("If the email exists, a reset link will be sent");

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

            // Simulate email sending (LOG ONLY)
            var resetLink = $"https://frontend/reset-password?token={token}";
            Console.WriteLine($"RESET LINK: {resetLink}");

            return Ok("If the email exists, a reset link will be sent");
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
                return BadRequest("Invalid or expired token");

            var user = _context.Users
                .FirstOrDefault(u => u.UserId == resetToken.UserId);

            if (user == null)
                return BadRequest("Invalid token");

            // Hash new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            resetToken.IsUsed = true;

            _context.SaveChanges();

            return Ok("Password reset successful");
        }



    }
}
