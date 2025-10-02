using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateUser(User user);

        // ✅ Get all users
        Task<IEnumerable<User>> GetAllAsync();

        // ✅ Get user by Id
        Task<User?> GetByIdAsync(int userId);

        // ✅ Update user
        Task<bool> UpdateAsync(User user);

        // ✅ Delete user
        Task<bool> DeleteAsync(int userId);

    }
}
