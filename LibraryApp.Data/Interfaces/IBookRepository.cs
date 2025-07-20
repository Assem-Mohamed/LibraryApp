using LibraryApp.Data.Entities;

namespace LibraryApp.Data.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<IEnumerable<Book>> SearchByCategoryAsync(string category);
        Task AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
