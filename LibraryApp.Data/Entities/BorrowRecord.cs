using LibraryApp.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Data.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; } = null!;
        public DateOnly BorrowDate { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly ReturnDate { get; set; }
        public BorrowStatus BorrowStatus { get; set; }
    }
}