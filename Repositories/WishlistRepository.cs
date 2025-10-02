using Dapper;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly DapperContext _context;

        public WishlistRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WishlistItem>> GetWishlist(int userId)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<WishlistItem>(
                "sp_GetWishlist",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AddToWishlist(int userId, int productId)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_AddToWishlist",
                new { UserId = userId, ProductId = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateWishlist(int wishlistId, int productId)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_UpdateWishlist",
                new { WishlistId = wishlistId, ProductId = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task RemoveFromWishlist(int userId, int productId)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_RemoveFromWishlist",
                new { UserId = userId, ProductId = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ClearWishlist(int userId)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                "sp_ClearWishlist",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
