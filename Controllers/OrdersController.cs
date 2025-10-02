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
                return BadRequest("Invalid order request");

            var orderId = await _orderService.CreateOrderAsync(request);
            return Ok(new { OrderId = orderId, Message = "Order created successfully" });
        }

        // ✅ Get Order By Id (مع العناصر)
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order not found");

            return Ok(order); // order.Items هترجع من الـ Repository
        }

        // ✅ Get Orders By Customer (مع العناصر)
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
            return Ok(orders);
        }

        // ✅ Get All Orders (Admin) (مع العناصر)
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders); // هيبقى فيها Items بعد تعديل الـ Repository
        }

        // ✅ Update Order Status (Admin)
        [HttpPut("status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto request)
        {
            var updated = await _orderService.UpdateOrderStatusAsync(request);
            if (!updated)
                return BadRequest("Failed to update order status");

            return Ok(new { Message = "Order status updated successfully" });
        }
    }
}
