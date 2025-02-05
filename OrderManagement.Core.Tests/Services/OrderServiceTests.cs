using Moq;
using OrderManagement.Core.Exceptions;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Models;
using OrderManagement.Core.Services;
using Xunit;

namespace OrderManagement.Core.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _mockRepository;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockRepository = new Mock<IOrderRepository>();
            _service = new OrderService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithValidId_ReturnsOrder()
        {
            // Arrange
            var order = new Order { Id = 1, CustomerName = "Test Customer", OrderDate = DateTime.Now, TotalAmount = 100 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _service.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetOrderByIdAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Order)null);

            // Act & Assert
            await Assert.ThrowsAsync<OrderManagementException>(() => _service.GetOrderByIdAsync(1));
        }

        [Theory]
        [InlineData("", "Customer name is required")]
        [InlineData("ab", "Customer name must be at least 3 characters long")]
        public async Task CreateOrderAsync_WithInvalidCustomerName_ThrowsException(string customerName, string expectedMessage)
        {
            // Arrange
            var order = new Order { CustomerName = customerName, OrderDate = DateTime.Now, TotalAmount = 100 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<OrderManagementException>(() => _service.CreateOrderAsync(order));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
} 