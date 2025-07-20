using LibraryApp.Data.Enums;

namespace LibraryApp.Models.DTOs
{
    public class UpdateFavoriteCategoriesDto
    {
        public List<Categories> FavoriteCategories { get; set; } = [];
    }
}
