using LibraryApp.Data.Enums;

namespace LibraryApp.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public BookStatus BookStatus { get; set; }

    }
}