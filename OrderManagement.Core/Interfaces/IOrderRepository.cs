using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderManagement.Core.Models;

namespace OrderManagement.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<OrderStatistics> GetStatisticsAsync();
    }
} 