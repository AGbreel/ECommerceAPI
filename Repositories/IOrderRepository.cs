using ECommerceAPI.DTOs;
using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(CreateOrderDto orderDto);
        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto);
        Task<OrderDetailDto?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDetailDto>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<OrderDetailDto>> GetAllOrdersAsync();
        Task<OrderShippingDto?> GetShippingInfoAsync(int orderId);
    }
}
