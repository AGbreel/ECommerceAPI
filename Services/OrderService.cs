using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task<int> CreateOrderAsync(CreateOrderDto orderDto)
            => _orderRepository.CreateOrderAsync(orderDto);

        public Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto statusDto)
            => _orderRepository.UpdateOrderStatusAsync(statusDto);

        public Task<OrderDetailDto?> GetOrderByIdAsync(int orderId)
            => _orderRepository.GetOrderByIdAsync(orderId);

        public Task<IEnumerable<OrderDetailDto>> GetOrdersByCustomerAsync(int customerId)
            => _orderRepository.GetOrdersByCustomerAsync(customerId);

        public Task<IEnumerable<OrderDetailDto>> GetAllOrdersAsync()
            => _orderRepository.GetAllOrdersAsync();

        public Task<OrderShippingDto?> GetShippingInfoAsync(int orderId)
            => _orderRepository.GetShippingInfoAsync(orderId);
    }
}
