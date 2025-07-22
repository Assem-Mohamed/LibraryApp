using LibraryApp.Data.Enums;

namespace LibraryApp.Models.DTOs
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateOnly BorrowDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public BorrowStatus BorrowStatus { get; set; }
    }
}
