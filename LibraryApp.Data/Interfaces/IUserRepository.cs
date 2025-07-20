using LibraryApp.Data.Entities;

namespace LibraryApp.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
