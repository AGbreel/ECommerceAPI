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
            try
            {
                var cart = await _cartRepo.GetCart(userId);
                return Ok(new { Success = true, Data = cart });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItem item)
        {
            if (item == null)
                return BadRequest(new { Success = false, Message = "Invalid cart item" });

            try
            {
                await _cartRepo.AddToCart(item.UserId, item.ProductId, item.Quantity);
                return Ok(new { Success = true, Message = "Added to cart" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{userId}/{productId}")]
        public async Task<IActionResult> UpdateCartItem(int userId, int productId, [FromBody] int quantity)
        {
            if (quantity <= 0)
                return BadRequest(new { Success = false, Message = "Quantity must be greater than 0" });

            try
            {
                await _cartRepo.UpdateCartItem(userId, productId, quantity);
                return Ok(new { Success = true, Message = "Cart item updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            try
            {
                await _cartRepo.RemoveFromCart(userId, productId);
                return Ok(new { Success = true, Message = "Item removed from cart successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("clear/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            try
            {
                await _cartRepo.ClearCart(userId);
                return Ok(new { Success = true, Message = "Cart cleared" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}
