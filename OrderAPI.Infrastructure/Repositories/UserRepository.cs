using Microsoft.EntityFrameworkCore;
using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Interfaces;
using OrderAPI.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appContext;

        public UserRepository(AppDbContext context)
        {
            _appContext = context;
        }

        public async Task AddAsync(User user)
        {
            await _appContext.Users.AddAsync(user);
            await _appContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _appContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user is null) return;

            _appContext.Users.Remove(user);
            await _appContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _appContext.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _appContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _appContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _appContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task UpdateAsync(User user)
        {
            _appContext.Users.Update(user);
            await _appContext.SaveChangesAsync();
        }

        public Task SaveChangesAsync()
        {
            return _appContext.SaveChangesAsync();
        }
    }
}
