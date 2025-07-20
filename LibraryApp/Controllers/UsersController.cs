using LibraryApp.Data.Entities;
using LibraryApp.Data.Enums;
using LibraryApp.Data.Interfaces;
using LibraryApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
