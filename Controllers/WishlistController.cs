using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Repositories;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistRepository _wishlistRepo;

        public WishlistController(IWishlistRepository wishlistRepo)
        {
            _wishlistRepo = wishlistRepo;
        }

        // ✅ Get Wishlist by UserId
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            var wishlist = await _wishlistRepo.GetWishlist(userId);

            return Ok(new
            {
                Success = true,
                Message = "Wishlist retrieved successfully",
                Data = wishlist
            });
        }

        // ✅ Add to Wishlist
        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] WishlistCreateDto item)
        {
            if (item == null)
                return BadRequest(new { Success = false, Message = "Invalid wishlist data" });

            await _wishlistRepo.AddToWishlist(item.UserId, item.ProductId);

            return Ok(new
            {
                Success = true,
                Message = "Product added to wishlist"
            });
        }

        // ✅ Update Wishlist Item
        [HttpPut("{wishlistId}")]
        public async Task<IActionResult> UpdateWishlist(int wishlistId, [FromBody] int productId)
        {
            await _wishlistRepo.UpdateWishlist(wishlistId, productId);

            return Ok(new
            {
                Success = true,
                Message = "Wishlist updated successfully"
            });
        }

        // ✅ Remove Specific Product from Wishlist
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int userId, int productId)
        {
            await _wishlistRepo.RemoveFromWishlist(userId, productId);

            return Ok(new
            {
                Success = true,
                Message = "Product removed from wishlist"
            });
        }

        // ✅ Clear Wishlist for User
        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearWishlist(int userId)
        {
            await _wishlistRepo.ClearWishlist(userId);

            return Ok(new
            {
                Success = true,
                Message = "Wishlist cleared successfully"
            });
        }
    }
}
