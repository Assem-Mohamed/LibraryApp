using LibraryApp.Data.Enums;

namespace LibraryApp.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public ICollection<Categories> FavoriteCategories { get; set; } = [];
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime CreatedAt { get; set; }
    }
}