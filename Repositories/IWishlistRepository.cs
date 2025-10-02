using ECommerceAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceAPI.Repositories
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<WishlistItem>> GetWishlist(int userId);
        Task AddToWishlist(int userId, int productId);
        Task UpdateWishlist(int wishlistId, int productId);
        Task RemoveFromWishlist(int userId, int productId);
        Task ClearWishlist(int userId);
    }
}
