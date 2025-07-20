using LibraryApp.Data.Data;
using LibraryApp.Data.Entities;
using LibraryApp.Data.Interfaces;
using LibraryApp.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly LibraryAppDbContext _context;
        public BorrowRecordRepository(LibraryAppDbContext context)
        {
            _context = context;
        }
        public async Task<BorrowRecord?> GetByIdAsync(int id)
        {
            return await _context.BorrowRecords.FindAsync(id);
        }
        public async Task<IEnumerable<BorrowRecord>> GetByUserIdAsync(int userId)
        {
            return await _context.BorrowRecords
                .Where(br => br.UserId == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
        {
            return await _context.BorrowRecords.ToListAsync();
        }
        public async Task AddAsync(BorrowRecord record)
        {
            await _context.BorrowRecords.AddAsync(record);
            await SaveChangesAsync();
        }
        public async Task UpdateAsync(BorrowRecord record)
        {
            _context.BorrowRecords.Update(record);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var record = await GetByIdAsync(id);
            if (record != null)
            {
                _context.BorrowRecords.Remove(record);
                await SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<BorrowRecord>> GetOverdueRecordsAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _context.BorrowRecords
                .Where(br => br.DueDate < today && br.BorrowStatus == BorrowStatus.Active)
                .ToListAsync();
        }
    }
}