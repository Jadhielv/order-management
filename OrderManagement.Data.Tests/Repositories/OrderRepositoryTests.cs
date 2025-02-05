using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Moq;
using OrderManagement.Core.Models;
using OrderManagement.Data.Context;
using OrderManagement.Data.Database;
using OrderManagement.Data.Repositories;
using Xunit;
using System.Linq.Expressions;
using Moq.Language.Flow;

namespace OrderManagement.Data.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly Mock<IDbConnectionFactory> _mockDb;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            _mockDb = new Mock<IDbConnectionFactory>();
            _repository = new OrderRepository(_mockDb.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOrders()
        {
            // Arrange
            var expectedOrders = new List<Order>
            {
                new() { Id = 1, CustomerName = "Test Customer", OrderDate = DateTime.Now, TotalAmount = 100 }
            };

            _mockDb.Setup(x => x.QueryAsync<Order>(
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync(expectedOrders);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedOrders.First().Id, result.First().Id);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsOrder()
        {
            // Arrange
            var expectedOrder = new Order { Id = 1, CustomerName = "Test Customer", OrderDate = DateTime.Now, TotalAmount = 100 };

            _mockDb.Setup(x => x.QueryFirstOrDefaultAsync<Order>(
                It.IsAny<string>(),
                It.IsAny<object>()
            )).ReturnsAsync(expectedOrder);

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrder.Id, result.Id);
        }
    }
} 