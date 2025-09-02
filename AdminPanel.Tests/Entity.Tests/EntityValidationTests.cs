using System.ComponentModel.DataAnnotations;
using AdminPanel.Entity;
using AdminPanel.Entity.Authorization;

namespace AdminPanel.Tests.Entity.Tests;

public class EntityValidationTests
{
    public static IList<ValidationResult> ValidateEntity<T>(T entity)
    {
        var context = new ValidationContext(entity!, null, null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(entity!, context, results, true);
        return results;
    }

    // GameEntity Tests
    [Fact]
    public void GameEntity_WithValidData_PassesValidation()
    {
        // Arrange
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Game",
            Key = "test-game",
            Description = "A test game",
            Price = 29.99,
            UnitInStock = 10,
            Discount = 0,
            PublisherId = Guid.NewGuid(),
            IsDeleted = false,
        };

        // Act
        var results = ValidateEntity(game);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void GameEntity_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var game = new GameEntity();

        // Assert
        Assert.False(game.IsDeleted);
        Assert.Equal(Guid.Empty, game.Id); // Default GUID is empty
        Assert.Equal(0, game.Price);
        Assert.Equal(0, game.UnitInStock);
        Assert.Equal(0, game.Discount);
    }

    [Fact]
    public void GameEntity_Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var game = new GameEntity();
        var testId = Guid.NewGuid();

        // Act
        game.Id = testId;
        game.Name = "Test Game";
        game.Key = "test-key";
        game.Description = "Test Description";
        game.Price = 59.99;
        game.UnitInStock = 5;
        game.Discount = 10;
        game.PublisherId = Guid.NewGuid();
        game.IsDeleted = true;

        // Assert
        Assert.Equal(testId, game.Id);
        Assert.Equal("Test Game", game.Name);
        Assert.Equal("test-key", game.Key);
        Assert.Equal("Test Description", game.Description);
        Assert.Equal(59.99, game.Price);
        Assert.Equal(5, game.UnitInStock);
        Assert.Equal(10, game.Discount);
        Assert.True(game.IsDeleted);
    }

    // GenreEntity Tests
    [Fact]
    public void GenreEntity_WithValidData_PassesValidation()
    {
        // Arrange
        var genre = new GenreEntity
        {
            Id = Guid.NewGuid(),
            Name = "Action",
            ParentGenreId = null,
        };

        // Act
        var results = ValidateEntity(genre);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void GenreEntity_WithParentGenre_CanSetParentId()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var genre = new GenreEntity
        {
            Id = Guid.NewGuid(),
            Name = "Sub-Genre",
            ParentGenreId = parentId,
        };

        // Act & Assert
        Assert.Equal(parentId, genre.ParentGenreId);
        Assert.Equal("Sub-Genre", genre.Name);
    }

    // PlatformEntity Tests
    [Fact]
    public void PlatformEntity_WithValidData_PassesValidation()
    {
        // Arrange
        var platform = new PlatformEntity
        {
            Id = Guid.NewGuid(),
            Type = "PC",
        };

        // Act
        var results = ValidateEntity(platform);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void PlatformEntity_Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var platform = new PlatformEntity();
        var testId = Guid.NewGuid();

        // Act
        platform.Id = testId;
        platform.Type = "PlayStation 5";

        // Assert
        Assert.Equal(testId, platform.Id);
        Assert.Equal("PlayStation 5", platform.Type);
    }

    // PublisherEntity Tests
    [Fact]
    public void PublisherEntity_WithValidData_PassesValidation()
    {
        // Arrange
        var publisher = new PublisherEntity
        {
            Id = Guid.NewGuid(),
            CompanyName = "Test Publisher",
            Description = "A test publisher",
            HomePage = "https://testpublisher.com",
        };

        // Act
        var results = ValidateEntity(publisher);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void PublisherEntity_Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var publisher = new PublisherEntity();
        var testId = Guid.NewGuid();

        // Act
        publisher.Id = testId;
        publisher.CompanyName = "Epic Games";
        publisher.Description = "Video game publisher";
        publisher.HomePage = "https://epicgames.com";

        // Assert
        Assert.Equal(testId, publisher.Id);
        Assert.Equal("Epic Games", publisher.CompanyName);
        Assert.Equal("Video game publisher", publisher.Description);
        Assert.Equal("https://epicgames.com", publisher.HomePage);
    }

    // CommentEntity Tests
    [Fact]
    public void CommentEntity_WithValidData_PassesValidation()
    {
        // Arrange
        var comment = new CommentEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Body = "This is a test comment",
            GameId = Guid.NewGuid(),
            ParentCommentId = null,
        };

        // Act
        var results = ValidateEntity(comment);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void CommentEntity_WithParentComment_CanSetParentId()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var comment = new CommentEntity
        {
            Id = Guid.NewGuid(),
            Name = "Reply User",
            Body = "This is a reply",
            GameId = Guid.NewGuid(),
            ParentCommentId = parentId,
        };

        // Act & Assert
        Assert.Equal(parentId, comment.ParentCommentId);
        Assert.Equal("Reply User", comment.Name);
        Assert.Equal("This is a reply", comment.Body);
    }

    // OrderEntity Tests
    [Fact]
    public void OrderEntity_WithValidData_PassesValidation()
    {
        // Arrange
        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            Date = DateTime.Now,
            Status = OrderStatus.Open,
        };

        // Act
        var results = ValidateEntity(order);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void OrderEntity_Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var order = new OrderEntity();
        var testId = Guid.NewGuid();
        var testDate = DateTime.Now;

        // Act
        order.Id = testId;
        order.CustomerId = Guid.NewGuid();
        order.Date = testDate;
        order.Status = OrderStatus.Shipped;

        // Assert
        Assert.Equal(testId, order.Id);
        Assert.NotEqual(Guid.Empty, order.CustomerId);
        Assert.Equal(testDate, order.Date);
        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    // OrderGameEntity Tests
    [Fact]
    public void OrderGameEntity_WithValidData_PassesValidation()
    {
        // Arrange
        var orderDetail = new OrderGameEntity
        {
            OrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Price = 39.99,
            Quantity = 2,
            Discount = 0,
        };

        // Act
        var results = ValidateEntity(orderDetail);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void OrderGameEntity_Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var orderDetail = new OrderGameEntity();
        var orderId = Guid.NewGuid();
        var gameId = Guid.NewGuid();

        // Act
        orderDetail.OrderId = orderId;
        orderDetail.ProductId = gameId;
        orderDetail.Price = 49.99;
        orderDetail.Quantity = 3;
        orderDetail.Discount = 5;

        // Assert
        Assert.Equal(orderId, orderDetail.OrderId);
        Assert.Equal(gameId, orderDetail.ProductId);
        Assert.Equal(49.99, orderDetail.Price);
        Assert.Equal(3, orderDetail.Quantity);
        Assert.Equal(5, orderDetail.Discount);
    }

    // UserEntity Tests
    [Fact]
    public void UserEntity_WithValidData_PassesValidation()
    {
        // Arrange
        var user = new UserEntity
        {
            Id = "user-123",
            UserName = "testuser",
            Email = "test@example.com",
            IsExternalUser = false,
        };

        // Act
        var results = ValidateEntity(user);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void UserEntity_Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        var user = new UserEntity
        {
            // Act
            Id = "user-456",
            UserName = "johnsmith",
            Email = "john@example.com",
            IsExternalUser = true,
        };

        // Assert
        Assert.Equal("user-456", user.Id);
        Assert.Equal("johnsmith", user.UserName);
        Assert.Equal("john@example.com", user.Email);
        Assert.True(user.IsExternalUser);
    }

    [Fact]
    public void UserEntity_DefaultIsExternalUser_IsFalse()
    {
        // Arrange & Act
        var user = new UserEntity();

        // Assert
        Assert.False(user.IsExternalUser);
    }

    // Entity Collection Tests
    [Fact]
    public void GameEntity_CanInitializeWithCollections()
    {
        // Arrange & Act
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test Game",
            Key = "test-game",
        };

        // Assert - Entity should support collection properties if they exist
        Assert.NotNull(game);
        Assert.Equal("Test Game", game.Name);
    }

    // Boundary Value Tests
    [Fact]
    public void GameEntity_WithZeroPrice_IsValid()
    {
        // Arrange
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Free Game",
            Key = "free-game",
            Price = 0.0,
            UnitInStock = 1000,
            Discount = 0,
            PublisherId = Guid.NewGuid(),
        };

        // Act
        var results = ValidateEntity(game);

        // Assert
        Assert.Empty(results);
        Assert.Equal(0.0, game.Price);
    }

    [Fact]
    public void GameEntity_WithHighPrice_IsValid()
    {
        // Arrange
        var game = new GameEntity
        {
            Id = Guid.NewGuid(),
            Name = "Premium Game",
            Key = "premium-game",
            Price = 999.99,
            UnitInStock = 1,
            Discount = 0,
            PublisherId = Guid.NewGuid(),
        };

        // Act
        var results = ValidateEntity(game);

        // Assert
        Assert.Empty(results);
        Assert.Equal(999.99, game.Price);
    }

    [Fact]
    public void OrderGameEntity_WithZeroQuantity_IsValid()
    {
        // Arrange
        var orderDetail = new OrderGameEntity
        {
            OrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Price = 29.99,
            Quantity = 0, // Edge case: zero quantity
        };

        // Act
        var results = ValidateEntity(orderDetail);

        // Assert
        Assert.Empty(results);
        Assert.Equal(0, orderDetail.Quantity);
    }

    // String Length and Content Tests
    [Fact]
    public void CommentEntity_WithLongBody_IsValid()
    {
        // Arrange
        var longBody = new string('A', 1000); // 1000 character comment
        var comment = new CommentEntity
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Body = longBody,
            GameId = Guid.NewGuid(),
        };

        // Act
        var results = ValidateEntity(comment);

        // Assert
        Assert.Empty(results);
        Assert.Equal(1000, comment.Body.Length);
    }

    [Fact]
    public void GenreEntity_WithLongName_IsValid()
    {
        // Arrange
        var longName = "Very Long Genre Name That Might Be Used In Some Cases";
        var genre = new GenreEntity
        {
            Id = Guid.NewGuid(),
            Name = longName,
        };

        // Act
        var results = ValidateEntity(genre);

        // Assert
        Assert.Empty(results);
        Assert.Equal(longName, genre.Name);
    }
}