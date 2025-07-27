using LibraryApp.Auth;
using LibraryApp.Data.Entities;
using LibraryApp.Data.Interfaces;
using LibraryApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace LibraryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtTokenGenerator _tokenGenerator;
        public AuthController(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, JwtTokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest("Email already registered.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = LibraryApp.Data.Enums.UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return Ok("Signup successful, please login.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
                return Unauthorized("Invalid credentials, user doesn't exist");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials, incorrect password");

            var token = _tokenGenerator.GenerateToken(user);
            return Ok(new { 
                Token = token,
                user.Role
            });
        }

        //[Authorize]
        //[HttpPost("change-password")]
        //public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        //{
        //    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        //    var user = await _userRepository.GetByIdAsync(userId);
        //    if (user == null)
        //        return Unauthorized();

        //    var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
        //    if (result == PasswordVerificationResult.Failed)
        //        return BadRequest("Old password is incorrect.");

        //    user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
        //    await _userRepository.SaveChangesAsync();

        //    return Ok("Password changed successfully.");
        //}

        [HttpPost("reset-password-public")]
        public async Task<IActionResult> ResetPasswordPublic(ResetPasswordPublicDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("Email not found.");
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            await _userRepository.SaveChangesAsync();
                
            return Ok("Password reset successful. You can now login.");
        }
    }
}
