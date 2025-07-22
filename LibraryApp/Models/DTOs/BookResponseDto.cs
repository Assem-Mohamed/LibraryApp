using LibraryApp.Data.Enums;

namespace LibraryApp.Models.DTOs
{
    public class BookResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public BookStatus BookStatus { get; set; }
    }
}
