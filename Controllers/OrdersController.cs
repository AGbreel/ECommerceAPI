using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ✅ Create Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return BadRequest(new { Success = false, Message = "Invalid order request" });

            try
            {
                var orderId = await _orderService.CreateOrderAsync(request);
                return Ok(new { Success = true, OrderId = orderId, Message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Get Order By Id (مع العناصر)
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                    return NotFound(new { Success = false, Message = "Order not found" });

                return Ok(new { Success = true, Data = order });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Get Orders By Customer (مع العناصر)
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomer(int customerId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
                return Ok(new { Success = true, Data = orders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Get All Orders (Admin) (مع العناصر)
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(new { Success = true, Data = orders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // ✅ Update Order Status (Admin)
        [HttpPut("status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto request)
        {
            if (request == null)
                return BadRequest(new { Success = false, Message = "Invalid request" });

            try
            {
                var updated = await _orderService.UpdateOrderStatusAsync(request);
                if (!updated)
                    return BadRequest(new { Success = false, Message = "Failed to update order status" });

                return Ok(new { Success = true, Message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}
