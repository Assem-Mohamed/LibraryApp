using LibraryApp.Data.Entities;
using LibraryApp.Data.Enums;
using LibraryApp.Data.Interfaces;
using LibraryApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("{id}/favorite-categories")]
        [Authorize]
        public async Task<IActionResult> UpdateFavoriteCategories(int id, UpdateFavoriteCategoriesDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            user.FavoriteCategories = dto.FavoriteCategories;
            await _userRepository.SaveChangesAsync();

            return Ok("Favorite categories updated successfully.");
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var currentRole = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            if (currentRole == "Librarian" && dto.Role != "Librarian")
                return Forbid("Librarians can only create other Librarians.");

            if (currentRole == "Admin" && dto.Role != "Admin" && dto.Role != "Librarian")
                return BadRequest("Admin can only create Admins or Librarians.");

            if (!Enum.TryParse<UserRole>(dto.Role, out var role))
                return BadRequest("Invalid role provided.");

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest("Email already registered.");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return Ok("User created successfully.");
        }
        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string? username, [FromQuery] int? id)
        {
            if (id is null && string.IsNullOrWhiteSpace(username))
                return BadRequest("You must provide either a username or an ID.");

            User? user = null;

            if (id.HasValue)
                user = await _userRepository.GetByIdAsync(id.Value);
            else if (!string.IsNullOrWhiteSpace(username))
                user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
                return NotFound("User not found.");

            var dto = new UserSummaryDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(dto);
        }
        
            [HttpPut("{id}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> EditUser(int id, EdituserDto dto)
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                if (!string.IsNullOrEmpty(dto.Username))
                    user.Username = dto.Username;

                if (!string.IsNullOrEmpty(dto.Role))
                {
                    if (!Enum.TryParse<UserRole>(dto.Role, true, out var parsedRole))
                        return BadRequest("Invalid role.");
                    user.Role = parsedRole;
                }

                await _userRepository.UpdateAsync(user);
                return Ok("User updated successfully.");
            }

          
            [HttpDelete("{id}")]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                await _userRepository.DeleteAsync(user);
                return Ok("User deleted successfully.");
            }
        }



    }


