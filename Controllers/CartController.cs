using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Repositories;
using ECommerceAPI.Models;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartRepo.GetCart(userId);
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItem item)
        {
            await _cartRepo.AddToCart(item.UserId, item.ProductId, item.Quantity);
            return Ok("Added to cart");
        }

        [HttpPut("{userId}/{productId}")]
        public async Task<IActionResult> UpdateCartItem(int userId, int productId, [FromBody] int quantity)
        {
            await _cartRepo.UpdateCartItem(userId, productId, quantity);
            return Ok("Cart item updated");
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            await _cartRepo.RemoveFromCart(userId, productId);
            return Ok("Item removed from cart successfully");
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartRepo.ClearCart(userId);
            return Ok("Cart cleared");
        }
    }
}
