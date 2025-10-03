using OrderAPI.Domain.Entities;
using OrderAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> ExistsByUsernameAsync(string username);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }

}
