using LibraryApp.Data.Enums;
using System.Globalization;

namespace LibraryApp.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string? CoverImageUrl { get; set; }
        public BookStatus BookStatus { get; set; }

    }
}