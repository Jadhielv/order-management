using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Models;
using OrderManagement.Core.Exceptions;

namespace OrderManagement.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new OrderManagementException($"Order with ID {id} not found", 404);
            return order;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            ValidateOrder(order);
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<Order> UpdateOrderAsync(int id, Order order)
        {
            ValidateOrder(order);
            
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            order.Id = id;
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<OrderStatistics> GetOrderStatisticsAsync()
        {
            return await _orderRepository.GetStatisticsAsync();
        }

        private void ValidateOrder(Order order)
        {
            if (string.IsNullOrWhiteSpace(order.CustomerName))
                throw new OrderManagementException("Customer name is required", 400);

            if (order.CustomerName.Length < 3)
                throw new OrderManagementException("Customer name must be at least 3 characters long", 400);

            if (order.OrderDate > DateTime.Now)
                throw new OrderManagementException("Order date cannot be in the future", 400);

            if (order.TotalAmount <= 0)
                throw new OrderManagementException("Total amount must be greater than zero", 400);
        }
    }
} 