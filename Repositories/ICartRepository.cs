using ECommerceAPI.DTOs;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories
{
    public interface ICartRepository
    {
        Task<CartResponseDto> GetCart(int userId);
        Task AddToCart(int userId, int productId, int quantity);
        Task UpdateCartItem(int userId, int productId, int quantity);
        Task RemoveFromCart(int userId, int productId);
        Task ClearCart(int userId);
    }
}
