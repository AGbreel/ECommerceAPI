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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWishlist(int userId)
        {
            var wishlist = await _wishlistRepo.GetWishlist(userId);
            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] WishlistCreateDto item)
        {
            if (item == null)
                return BadRequest("Invalid wishlist data");

            // استدعاء الريبو
            await _wishlistRepo.AddToWishlist(item.UserId, item.ProductId);
            return Ok(new { message = "Product added to wishlist" });
        }


        [HttpPut("{wishlistId}")]
        public async Task<IActionResult> UpdateWishlist(int wishlistId, [FromBody] int productId)
        {
            await _wishlistRepo.UpdateWishlist(wishlistId, productId);
            return Ok("Wishlist updated");
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromWishlist(int userId, int productId)
        {
            await _wishlistRepo.RemoveFromWishlist(userId, productId);
            return Ok("Removed from wishlist");
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearWishlist(int userId)
        {
            await _wishlistRepo.ClearWishlist(userId);
            return Ok("Wishlist cleared");
        }
    }
}
