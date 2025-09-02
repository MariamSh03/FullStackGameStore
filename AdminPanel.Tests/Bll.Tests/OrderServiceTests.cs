using System.Linq.Expressions;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Services;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using Moq;

namespace AdminPanel.Tests.Bll.Tests;
public class OrderServiceTests
{
    private readonly Mock<IGameRepository> _mockGameRepo = new();
    private readonly Mock<IGenericRepository<OrderEntity>> _mockOrderRepo = new();
    private readonly Mock<IGenericRepository<OrderGameEntity>> _mockOrderGameRepo = new();
#pragma warning disable IDE0052 // Remove unread private members
    private readonly Mock<HttpClient> _mockHttpClient = new();
#pragma warning restore IDE0052 // Remove unread private members
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _orderService = new OrderService(
            _mockGameRepo.Object,
            _mockOrderRepo.Object,
            _mockOrderGameRepo.Object,
            new HttpClient()); // Real client used unless testing ProcessPaymentAsync
    }

    [Fact]
    public async Task AddGameToCartAsync_GameNotFound_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockGameRepo.Setup(r => r.GetByIdAsync(gameId))
            .ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() =>
            _orderService.AddGameToCartAsync(gameId));
    }

    [Fact]
    public async Task AddGameToCartAsync_GameIsDeleted_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var deletedGame = new GameEntity { Id = gameId, Name = "Deleted Game", IsDeleted = true };
        _mockGameRepo.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(deletedGame);

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _orderService.AddGameToCartAsync(gameId));
    }

    [Fact]
    public async Task AddGameToCartAsync_GameOutOfStock_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var game = new GameEntity { Id = gameId, Name = "Out of Stock Game", UnitInStock = 0, IsDeleted = false };
        _mockGameRepo.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(game);

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _orderService.AddGameToCartAsync(gameId));
    }

    [Fact]
    public async Task AddGameToCartAsync_GameAlreadyInCart_IncrementsQuantity()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var cartId = Guid.NewGuid();
        var game = new GameEntity { Id = gameId, Name = "Test Game", UnitInStock = 5, IsDeleted = false, Price = 10.0 };

        var existingOrderGame = new OrderGameEntity
        {
            OrderId = cartId,
            ProductId = gameId,
            Quantity = 1,
            Price = 10.0,
        };

        _mockGameRepo.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(game);

        // Mock GetOrCreateCartAsync behavior
        var cart = new OrderEntity { Id = cartId, Status = OrderStatus.Open };
        _mockOrderRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OrderEntity, bool>>>()))
            .ReturnsAsync(new List<OrderEntity> { cart });

        _mockOrderGameRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OrderGameEntity, bool>>>()))
            .ReturnsAsync(new List<OrderGameEntity> { existingOrderGame });

        _mockOrderGameRepo.Setup(r => r.UpdateAsync(It.IsAny<OrderGameEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        await _orderService.AddGameToCartAsync(gameId);

        // Assert
        Assert.Equal(2, existingOrderGame.Quantity);
        Assert.Equal(4, game.UnitInStock);
        _mockOrderGameRepo.Verify(r => r.UpdateAsync(existingOrderGame), Times.Once);
    }

    [Fact]
    public async Task AddGameToCartAsync_ExceedsStock_ThrowsException()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var cartId = Guid.NewGuid();
        var game = new GameEntity { Id = gameId, Name = "Test Game", UnitInStock = 2, IsDeleted = false, Price = 10.0 };

        var existingOrderGame = new OrderGameEntity
        {
            OrderId = cartId,
            ProductId = gameId,
            Quantity = 3, // More than available stock
            Price = 10.0,
        };

        _mockGameRepo.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(game);
        var cart = new OrderEntity { Id = cartId, Status = OrderStatus.Open };
        _mockOrderRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OrderEntity, bool>>>()))
            .ReturnsAsync(new List<OrderEntity> { cart });

        _mockOrderGameRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OrderGameEntity, bool>>>()))
            .ReturnsAsync(new List<OrderGameEntity> { existingOrderGame });

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _orderService.AddGameToCartAsync(gameId));
    }

    [Fact]
    public async Task AddGameToCartAsync_NewGameToCart_AddsOrderGame()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var cartId = Guid.NewGuid();
        var game = new GameEntity { Id = gameId, Name = "Test Game", UnitInStock = 5, IsDeleted = false, Price = 10.0 };

        _mockGameRepo.Setup(r => r.GetByIdAsync(gameId)).ReturnsAsync(game);

        var cart = new OrderEntity { Id = cartId, Status = OrderStatus.Open };
        _mockOrderRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OrderEntity, bool>>>()))
            .ReturnsAsync(new List<OrderEntity> { cart });

        _mockOrderGameRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OrderGameEntity, bool>>>()))
            .ReturnsAsync(new List<OrderGameEntity>()); // No existing items

        _mockOrderGameRepo.Setup(r => r.AddAsync(It.IsAny<OrderGameEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        await _orderService.AddGameToCartAsync(gameId);

        // Assert
        _mockOrderGameRepo.Verify(
            r => r.AddAsync(It.Is<OrderGameEntity>(og =>
                og.OrderId == cartId &&
                og.ProductId == gameId &&
                og.Quantity == 1 &&
                og.Price == 10.0)),
            Times.Once);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ValidId_ReturnsOrderDto()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new OrderEntity { Id = orderId, CustomerId = Guid.NewGuid(), Date = DateTime.UtcNow };

        _mockOrderRepo.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act
        var result = await _orderService.GetOrderByIdAsync(orderId);

        // Assert
        Assert.Equal(order.Id, result.Id);
        Assert.Equal(order.CustomerId, result.CustomerId);
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsAllOrders()
    {
        // Arrange
        var orders = new List<OrderEntity>
        {
            new() { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.UtcNow },
        };

        _mockOrderRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(orders);

        // Act
        var result = await _orderService.GetOrdersAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task RemoveGameFromCartAsync_GameNotFound_ThrowsException()
    {
        // Arrange
        var cart = new OrderEntity { Id = Guid.NewGuid(), Status = OrderStatus.Open };
        _mockOrderRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OrderEntity, bool>>>()))
            .ReturnsAsync(new List<OrderEntity> { cart });

        _mockGameRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity> { new() { Id = Guid.NewGuid(), Key = "test" } });

        _mockOrderGameRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<OrderGameEntity, bool>>>()))
            .ReturnsAsync(new List<OrderGameEntity>()); // not found

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() =>
            _orderService.RemoveGameFromCartAsync("test"));
    }
}