using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderDto orderDto);
        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto statusDto);
        Task<OrderDetailDto?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDetailDto>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<OrderDetailDto>> GetAllOrdersAsync();
        Task<OrderShippingDto?> GetShippingInfoAsync(int orderId);
    }
}
