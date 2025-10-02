using Dapper;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        // ✅ Register User via Stored Procedure
        public async Task<User> CreateUser(User user)
        {
            var query = "sp_CreateUser";
            var parameters = new DynamicParameters();
            parameters.Add("FullName", user.FullName, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("PasswordHash", user.PasswordHash, DbType.String);
            parameters.Add("Role", user.Role, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                // نخلي الـ proc يرجع الـ UserId الجديد
                var userId = await connection.ExecuteScalarAsync<int>(
                    query, parameters, commandType: CommandType.StoredProcedure
                );

                user.UserId = userId;
                return user;
            }
        }

        // ✅ Get user by email
        public async Task<User?> GetByEmailAsync(string email)
        {
            var query = "sp_GetUserByEmail"; // اعمل procedure يرجع user based on Email
            var parameters = new DynamicParameters();
            parameters.Add("Email", email, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    query, parameters, commandType: CommandType.StoredProcedure
                );
            }
        }

        // ✅ Get all users
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var query = "sp_GetAllUsers";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<User>(
                    query, commandType: CommandType.StoredProcedure
                );
            }
        }

        // ✅ Get user by Id
        public async Task<User?> GetByIdAsync(int userId)
        {
            var query = "sp_GetUserById";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(
                    query, parameters, commandType: CommandType.StoredProcedure
                );
            }
        }

        // ✅ Update user
        public async Task<bool> UpdateAsync(User user)
        {
            var query = "sp_UpdateUserFlexible";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", user.UserId, DbType.Int32);
            parameters.Add("FullName", user.FullName, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("PasswordHash", user.PasswordHash, DbType.String);
            parameters.Add("Role", user.Role, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                var rows = await connection.ExecuteAsync(
                    query, parameters, commandType: CommandType.StoredProcedure
                );
                return rows > 0;
            }
        }

        // ✅ Delete user
        public async Task<bool> DeleteAsync(int userId)
        {
            var query = "sp_DeleteUser";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                var rows = await connection.ExecuteAsync(
                    query, parameters, commandType: CommandType.StoredProcedure
                );
                return rows > 0;
            }
        }
    }
}
