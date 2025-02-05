using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Models;
using OrderManagement.Data.Database;

namespace OrderManagement.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionFactory _db;

        public OrderRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            const string sql = @"
                SELECT Id, CustomerName, OrderDate, TotalAmount, CreatedAt, UpdatedAt, IsDeleted 
                FROM Orders 
                WHERE IsDeleted = 0";
            return await _db.QueryAsync<Order>(sql);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, CustomerName, OrderDate, TotalAmount, CreatedAt, UpdatedAt, IsDeleted 
                FROM Orders 
                WHERE Id = @Id AND IsDeleted = 0";
            return await _db.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
        }

        public async Task<Order> CreateAsync(Order order)
        {
            const string sql = @"
                INSERT INTO Orders (CustomerName, OrderDate, TotalAmount, CreatedAt, IsDeleted) 
                VALUES (@CustomerName, @OrderDate, @TotalAmount, GETDATE(), 0);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            order.Id = await _db.ExecuteScalarAsync<int>(sql, order);
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            const string sql = @"
                UPDATE Orders 
                SET CustomerName = @CustomerName, 
                    OrderDate = @OrderDate, 
                    TotalAmount = @TotalAmount,
                    UpdatedAt = GETDATE()
                WHERE Id = @Id AND IsDeleted = 0";

            await _db.ExecuteAsync(sql, order);
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"
                UPDATE Orders 
                SET IsDeleted = 1, 
                    UpdatedAt = GETDATE()
                WHERE Id = @Id AND IsDeleted = 0";

            var rowsAffected = await _db.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<OrderStatistics> GetStatisticsAsync()
        {
            const string sql = "SELECT * FROM OrderStatistics";
            return await _db.QueryFirstOrDefaultAsync<OrderStatistics>(sql);
        }
    }
} 