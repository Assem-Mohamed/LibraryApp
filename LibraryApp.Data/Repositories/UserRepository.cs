using LibraryApp.Data.Data;
using LibraryApp.Data.Entities;
using LibraryApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryAppDbContext _context;
        public UserRepository(LibraryAppDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
