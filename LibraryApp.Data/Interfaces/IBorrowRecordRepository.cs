using LibraryApp.Data.Entities;

namespace LibraryApp.Data.Interfaces
{
    public interface IBorrowRecordRepository
    {
        Task<BorrowRecord?> GetByIdAsync(int id);
        Task<IEnumerable<BorrowRecord>> GetByUserIdAsync(int userId);
        Task<IEnumerable<BorrowRecord>> GetAllAsync();
        Task AddAsync(BorrowRecord record);
        Task UpdateAsync(BorrowRecord record);
        Task SaveChangesAsync();
        Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync(DateOnly today);
    }
}
