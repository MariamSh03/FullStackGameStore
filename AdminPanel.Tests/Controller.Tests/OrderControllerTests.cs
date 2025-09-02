using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;

public class OrderControllerTests : IDisposable
{
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _mockOrderService = new Mock<IOrderService>();
        _controller = new OrderController(_mockOrderService.Object);
    }

    // GetOrders Tests
    [Fact]
    public async Task GetOrders_WhenOrdersExist_ReturnsOkWithOrders()
    {
        // Arrange
        var orders = new List<OrderDto>
        {
            new() { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.Now },
            new() { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.Now.AddDays(-1) },
        };
        _mockOrderService.Setup(s => s.GetOrdersAsync()).ReturnsAsync(orders);

        // Act
        var result = await _controller.GetOrders();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOrders = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(okResult.Value);
        Assert.Equal(2, returnedOrders.Count());
    }

    [Fact]
    public async Task GetOrders_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        _mockOrderService.Setup(s => s.GetOrdersAsync())
                        .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _controller.GetOrders();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Database error", badRequestResult.Value);
    }

    // GetOrderById Tests
    [Fact]
    public async Task GetOrderById_WhenOrderExists_ReturnsOkWithOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new OrderDto { Id = orderId, CustomerId = Guid.NewGuid(), Date = DateTime.Now };
        _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId)).ReturnsAsync(order);

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOrder = Assert.IsType<OrderDto>(okResult.Value);
        Assert.Equal(orderId, returnedOrder.Id);
    }

    [Fact]
    public async Task GetOrderById_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId))
                        .ThrowsAsync(new KeyNotFoundException("Order not found"));

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Order not found", badRequestResult.Value);
    }

    // GetOrderDetails Tests
    [Fact]
    public async Task GetOrderDetails_WhenDetailsExist_ReturnsOkWithDetails()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var details = new List<OrderGameDto>
        {
            new() { GameId = Guid.NewGuid(), GameName = "Game 1", Quantity = 2, Price = 29.99 },
            new() { GameId = Guid.NewGuid(), GameName = "Game 2", Quantity = 1, Price = 59.99 },
        };
        _mockOrderService.Setup(s => s.GetOrderDetailsAsync(orderId)).ReturnsAsync(details);

        // Act
        var result = await _controller.GetOrderDetails(orderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDetails = Assert.IsAssignableFrom<IEnumerable<OrderGameDto>>(okResult.Value);
        Assert.Equal(2, returnedDetails.Count());
    }

    [Fact]
    public async Task GetOrderDetails_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.GetOrderDetailsAsync(orderId))
                        .ThrowsAsync(new InvalidOperationException("Details not found"));

        // Act
        var result = await _controller.GetOrderDetails(orderId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Details not found", badRequestResult.Value);
    }

    // GetCart Tests
    [Fact]
    public async Task GetCart_WhenCartExists_ReturnsOkWithCart()
    {
        // Arrange
        var cart = new List<OrderGameDto>
        {
            new() { GameId = Guid.NewGuid(), GameName = "Cart Game", Quantity = 1, Price = 39.99 },
        };
        _mockOrderService.Setup(s => s.GetCartAsync()).ReturnsAsync(cart);

        // Act
        var result = await _controller.GetCart();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCart = Assert.IsAssignableFrom<IEnumerable<OrderGameDto>>(okResult.Value);
        Assert.NotEmpty(returnedCart);
    }

    [Fact]
    public async Task GetCart_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        _mockOrderService.Setup(s => s.GetCartAsync())
                        .ThrowsAsync(new UnauthorizedAccessException("User not found"));

        // Act
        var result = await _controller.GetCart();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("User not found", badRequestResult.Value);
    }

    // AddGameToCart Tests
    [Fact]
    public async Task AddGameToCart_WhenSuccessful_ReturnsOkWithMessage()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.AddGameToCartAsync(gameId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddGameToCart(gameId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        var messageProperty = response.GetType().GetProperty("Message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Game added to cart.", messageProperty.GetValue(response));
    }

    [Fact]
    public async Task AddGameToCart_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.AddGameToCartAsync(gameId))
                        .ThrowsAsync(new InvalidOperationException("Game not available"));

        // Act
        var result = await _controller.AddGameToCart(gameId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Game not available", badRequestResult.Value);
    }

    // RemoveGameFromCart Tests
    [Fact]
    public async Task RemoveGameFromCart_WhenSuccessful_ReturnsNoContent()
    {
        // Arrange
        var gameKey = "test-game";
        _mockOrderService.Setup(s => s.RemoveGameFromCartAsync(gameKey)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RemoveGameFromCart(gameKey);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task RemoveGameFromCart_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var gameKey = "test-game";
        _mockOrderService.Setup(s => s.RemoveGameFromCartAsync(gameKey))
                        .ThrowsAsync(new KeyNotFoundException("Game not in cart"));

        // Act
        var result = await _controller.RemoveGameFromCart(gameKey);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Game not in cart", badRequestResult.Value);
    }

    // GetPaymentMethods Tests
    [Fact]
    public async Task GetPaymentMethods_WhenMethodsExist_ReturnsOkWithMethods()
    {
        // Arrange
        var methods = new PaymentMethodsDto
        {
            PaymentMethods = new List<PaymentMethodDto>
            {
                new() { Title = "Credit Card", Description = "Visa/MasterCard", ImageUrl = "/images/card.png" },
                new() { Title = "PayPal", Description = "PayPal Account", ImageUrl = "/images/paypal.png" },
                new() { Title = "Bank Transfer", Description = "Direct Transfer", ImageUrl = "/images/bank.png" },
            },
        };
        _mockOrderService.Setup(s => s.GetPaymentMethodsAsync()).ReturnsAsync(methods);

        // Act
        var result = await _controller.GetPaymentMethods();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMethods = Assert.IsType<PaymentMethodsDto>(okResult.Value);
        Assert.Equal(3, returnedMethods.PaymentMethods.Count);
        Assert.Contains(returnedMethods.PaymentMethods, m => m.Title == "Credit Card");
    }

    [Fact]
    public async Task GetPaymentMethods_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        _mockOrderService.Setup(s => s.GetPaymentMethodsAsync())
                        .ThrowsAsync(new InvalidOperationException("Payment service unavailable"));

        // Act
        var result = await _controller.GetPaymentMethods();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Payment service unavailable", badRequestResult.Value);
    }

    // ProcessPayment Tests
    [Fact]
    public async Task ProcessPayment_WhenSuccessful_ReturnsOkWithResult()
    {
        // Arrange
        var request = new PaymentRequestDto
        {
            Method = "Credit Card",
        };
        var paymentResult = new { Success = true, TransactionId = "TXN123" };
        _mockOrderService.Setup(s => s.ProcessPaymentAsync(request)).ReturnsAsync(paymentResult);

        // Act
        var result = await _controller.ProcessPayment(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    // [Fact]
    //  public async Task ProcessPayment_WhenReturningPdf_ReturnsFileResult()
    //  {
    //    // Arrange
    //    var request = new PaymentRequestDto
    //    {
    //        Method = "Bank Transfer",
    //    };
    //    var pdfStream = new MemoryStream();
    //    var fileName = "invoice.pdf";
    //    var pdfResult = (object)(pdfStream, fileName);
    //    _mockOrderService.Setup(s => s.ProcessPaymentAsync(request)).ReturnsAsync(pdfResult);

    // // Act
    //    var result = await _controller.ProcessPayment(request);

    // Assert
    //    var fileResult = Assert.IsType<FileStreamResult>(result);
    //    Assert.Equal("application/pdf", fileResult.ContentType);
    //    Assert.Equal(fileName, fileResult.FileDownloadName);
    //  }
    [Fact]
    public async Task ProcessPayment_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var request = new PaymentRequestDto
        {
            Method = "Invalid",
        };
        _mockOrderService.Setup(s => s.ProcessPaymentAsync(request))
                        .ThrowsAsync(new ArgumentException("Invalid payment method"));

        // Act
        var result = await _controller.ProcessPayment(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid payment method", badRequestResult.Value);
    }

    // UpdateOrderDetailQuantity Tests
    [Fact]
    public async Task UpdateOrderDetailQuantity_WhenSuccessful_ReturnsOkWithMessage()
    {
        // Arrange
        var detailId = Guid.NewGuid();
        var request = new UpdateQuantityRequestDto { Count = 3 };
        _mockOrderService.Setup(s => s.UpdateOrderDetailQuantityAsync(detailId, request.Count))
                        .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateOrderDetailQuantity(detailId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        var messageProperty = response.GetType().GetProperty("Message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Order detail quantity updated successfully.", messageProperty.GetValue(response));
    }

    [Fact]
    public async Task UpdateOrderDetailQuantity_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var detailId = Guid.NewGuid();
        var request = new UpdateQuantityRequestDto { Count = 0 };
        _mockOrderService.Setup(s => s.UpdateOrderDetailQuantityAsync(detailId, request.Count))
                        .ThrowsAsync(new ArgumentException("Invalid quantity"));

        // Act
        var result = await _controller.UpdateOrderDetailQuantity(detailId, request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid quantity", badRequestResult.Value);
    }

    // DeleteOrderDetail Tests
    [Fact]
    public async Task DeleteOrderDetail_WhenSuccessful_ReturnsOkWithMessage()
    {
        // Arrange
        var detailId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.DeleteOrderDetailAsync(detailId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteOrderDetail(detailId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        var messageProperty = response.GetType().GetProperty("Message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Order details delated successfully.", messageProperty.GetValue(response)); // Note: typo in original
    }

    [Fact]
    public async Task DeleteOrderDetail_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var detailId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.DeleteOrderDetailAsync(detailId))
                        .ThrowsAsync(new KeyNotFoundException("Order detail not found"));

        // Act
        var result = await _controller.DeleteOrderDetail(detailId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Order detail not found", badRequestResult.Value);
    }

    // ShipOrder Tests
    [Fact]
    public async Task ShipOrder_WhenSuccessful_ReturnsOkWithMessage()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.ShipOrderAsync(orderId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ShipOrder(orderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        var messageProperty = response.GetType().GetProperty("Message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Order shipped successfully.", messageProperty.GetValue(response));
    }

    [Fact]
    public async Task ShipOrder_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.ShipOrderAsync(orderId))
                        .ThrowsAsync(new InvalidOperationException("Order already shipped"));

        // Act
        var result = await _controller.ShipOrder(orderId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Order already shipped", badRequestResult.Value);
    }

    // AddGameToOrder Tests
    [Fact]
    public async Task AddGameToOrder_WhenSuccessful_ReturnsOkWithMessage()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var gameKey = "test-game";
        _mockOrderService.Setup(s => s.AddGameToOrderAsync(orderId, gameKey)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddGameToOrder(orderId, gameKey);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        var messageProperty = response.GetType().GetProperty("Message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Game added to order successfully.", messageProperty.GetValue(response));
    }

    [Fact]
    public async Task AddGameToOrder_WhenServiceThrows_ReturnsBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var gameKey = "invalid-game";
        _mockOrderService.Setup(s => s.AddGameToOrderAsync(orderId, gameKey))
                        .ThrowsAsync(new KeyNotFoundException("Game not found"));

        // Act
        var result = await _controller.AddGameToOrder(orderId, gameKey);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Game not found", badRequestResult.Value);
    }

    // Edge Cases and Error Handling Tests
    [Fact]
    public async Task GetOrders_WithEmptyResult_ReturnsOkWithEmptyList()
    {
        // Arrange
        var emptyOrders = new List<OrderDto>();
        _mockOrderService.Setup(s => s.GetOrdersAsync()).ReturnsAsync(emptyOrders);

        // Act
        var result = await _controller.GetOrders();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOrders = Assert.IsAssignableFrom<IEnumerable<OrderDto>>(okResult.Value);
        Assert.Empty(returnedOrders);
    }

    [Fact]
    public async Task ProcessPayment_WithNullRequest_ServiceHandlesValidation()
    {
        // Arrange
        PaymentRequestDto nullRequest = null;
        _mockOrderService.Setup(s => s.ProcessPaymentAsync(nullRequest!))
                        .ThrowsAsync(new ArgumentNullException(nameof(nullRequest)));

        // Act
        var result = await _controller.ProcessPayment(nullRequest!);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Value cannot be null", badRequestResult.Value.ToString());
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        // Controllers don't implement IDisposable in this case
    }

    // Additional tests for better branch coverage
    [Fact]
    public async Task GetOrderById_WhenInvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId))
                        .ThrowsAsync(new InvalidOperationException("Order not found"));

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Order not found", badRequestResult.Value);
    }

    [Fact]
    public async Task GetOrderDetails_WhenInvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.GetOrderDetailsAsync(orderId))
                        .ThrowsAsync(new InvalidOperationException("Order details not found"));

        // Act
        var result = await _controller.GetOrderDetails(orderId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Order details not found", badRequestResult.Value);
    }

    [Fact]
    public async Task GetCart_WhenInvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        _mockOrderService.Setup(s => s.GetCartAsync())
                        .ThrowsAsync(new InvalidOperationException("Cart error"));

        // Act
        var result = await _controller.GetCart();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Cart error", badRequestResult.Value);
    }

    [Fact]
    public async Task AddGameToCart_WhenInvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        _mockOrderService.Setup(s => s.AddGameToCartAsync(gameId))
                        .ThrowsAsync(new InvalidOperationException("Game not available"));

        // Act
        var result = await _controller.AddGameToCart(gameId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Game not available", badRequestResult.Value);
    }

    [Fact]
    public async Task RemoveGameFromCart_WhenInvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        var gameKey = "test-key";
        _mockOrderService.Setup(s => s.RemoveGameFromCartAsync(gameKey))
                        .ThrowsAsync(new InvalidOperationException("Game not in cart"));

        // Act
        var result = await _controller.RemoveGameFromCart(gameKey);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Game not in cart", badRequestResult.Value);
    }

    [Fact]
    public async Task GetPaymentMethods_WhenInvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        _mockOrderService.Setup(s => s.GetPaymentMethodsAsync())
                        .ThrowsAsync(new InvalidOperationException("Payment methods unavailable"));

        // Act
        var result = await _controller.GetPaymentMethods();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Payment methods unavailable", badRequestResult.Value);
    }
}