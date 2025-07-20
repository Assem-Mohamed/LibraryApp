using LibraryApp.Data.Data;
using LibraryApp.Data.Entities;
using LibraryApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryAppDbContext _context;
        public BookRepository(LibraryAppDbContext context)
        {
            _context = context;
        }
        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<IEnumerable<Book>> SearchByCategoryAsync(string category)
        {
            return await _context.Books
                .Where(b => b.Category == category)
                .ToListAsync();
        }
        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await SaveChangesAsync();
        }
        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await SaveChangesAsync();
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
