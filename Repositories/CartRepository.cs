using Dapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace ECommerceAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DapperContext _context;

        public CartRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<CartResponseDto> GetCart(int userId)
        {
            using var connection = _context.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "sp_GetCart",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            var items = (await multi.ReadAsync<CartProductDto>()).ToList();
            var total = await multi.ReadFirstOrDefaultAsync<decimal>();

            return new CartResponseDto
            {
                Items = items,
                CartTotal = total
            };
        }



        public async Task AddToCart(int userId, int productId, int quantity)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("sp_AddToCart", new { UserId = userId, ProductId = productId, Quantity = quantity }, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateCartItem(int userId, int productId, int quantity)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("sp_UpdateCartItem", new { UserId = userId, ProductId = productId, Quantity = quantity }, commandType: CommandType.StoredProcedure);
        }

        public async Task RemoveFromCart(int userId, int productId)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("sp_RemoveFromCart", new { UserId = userId, ProductId = productId }, commandType: CommandType.StoredProcedure);
        }

        public async Task ClearCart(int userId)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("sp_ClearCart", new { UserId = userId }, commandType: CommandType.StoredProcedure);
        }
    }
}
