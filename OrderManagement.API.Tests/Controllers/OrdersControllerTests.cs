using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagement.API.Controllers;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Models;
using Xunit;

namespace OrderManagement.API.Tests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrdersController(_mockOrderService.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsOkResult_WithOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new() { Id = 1, CustomerName = "Test Customer", OrderDate = DateTime.Now, TotalAmount = 100 }
            };
            _mockOrderService.Setup(s => s.GetAllOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrders = Assert.IsAssignableFrom<IEnumerable<Order>>(okResult.Value);
            Assert.Single(returnedOrders);
        }

        [Fact]
        public async Task GetOrder_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var order = new Order { Id = 1, CustomerName = "Test Customer", OrderDate = DateTime.Now, TotalAmount = 100 };
            _mockOrderService.Setup(s => s.GetOrderByIdAsync(1)).ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrder(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrder = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(1, returnedOrder.Id);
        }

        [Fact]
        public async Task CreateOrder_WithValidOrder_ReturnsCreatedAtAction()
        {
            // Arrange
            var order = new Order { CustomerName = "Test Customer", OrderDate = DateTime.Now, TotalAmount = 100 };
            var createdOrder = new Order { Id = 1, CustomerName = "Test Customer", OrderDate = DateTime.Now, TotalAmount = 100 };
            _mockOrderService.Setup(s => s.CreateOrderAsync(order)).ReturnsAsync(createdOrder);

            // Act
            var result = await _controller.CreateOrder(order);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedOrder = Assert.IsType<Order>(createdAtActionResult.Value);
            Assert.Equal(1, returnedOrder.Id);
        }
    }
} 