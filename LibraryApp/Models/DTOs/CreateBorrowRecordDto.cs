namespace LibraryApp.Models.DTOs
{
    public class CreateBorrowRecordDto
    {
        public int BookId { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly DueDate { get; set; }
    }
}
