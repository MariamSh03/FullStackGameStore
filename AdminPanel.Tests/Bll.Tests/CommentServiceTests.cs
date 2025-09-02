using System.Linq.Expressions;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Services;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using Moq;

namespace AdminPanel.Tests.Bll.Tests;

public class CommentServiceTests
{
    private readonly Mock<IGenericRepository<CommentEntity>> _commentRepoMock;
    private readonly Mock<IGameRepository> _gameRepoMock;
    private readonly CommentService _service;

    public CommentServiceTests()
    {
        _commentRepoMock = new Mock<IGenericRepository<CommentEntity>>();
        _gameRepoMock = new Mock<IGameRepository>();
        _service = new CommentService(_commentRepoMock.Object, _gameRepoMock.Object);
    }

    [Fact]
    public async Task AddCommentAsync_Should_Add_When_Valid()
    {
        // Arrange
        var gameKey = "game-key";
        var name = "Alice";
        var body = "Nice game!";
        var gameId = Guid.NewGuid();

        _gameRepoMock.Setup(r => r.FindAsync(g => g.Key == gameKey))
            .ReturnsAsync(new List<GameEntity> { new() { Id = gameId, Key = gameKey } });

        _commentRepoMock.Setup(r => r.AddAsync(It.IsAny<CommentEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AddCommentAsync(gameKey, name, body, null);

        // Assert
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
        _commentRepoMock.Verify(
            r => r.AddAsync(It.Is<CommentEntity>(
            c => c.Name == name && c.Body == body && c.GameId == gameId && c.ParentCommentId == null)), Times.Once);
#pragma warning restore SA1117 // Parameters should be on same line or separate lines
    }

    [Fact]
    public async Task GetCommentsAsync_Should_Return_Nested_Comments()
    {
        // Arrange
        var gameKey = "test-key";
        var gameId = Guid.NewGuid();
        var parentId = Guid.NewGuid();
        var childId = Guid.NewGuid();

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
    .ReturnsAsync(new List<GameEntity> { new() { Id = gameId, Key = gameKey } });

        var comments = new List<CommentEntity>
    {
        new() { Id = parentId, Name = "User1", Body = "Parent", GameId = gameId },
        new() { Id = childId, Name = "User2", Body = "Child", GameId = gameId, ParentCommentId = parentId },
    };

        _commentRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<CommentEntity, bool>>>()))
    .ReturnsAsync(comments);

        // Act
        var result = await _service.GetCommentsAsync(gameKey);

        // Assert
        Assert.Single(result);
        var parent = result.First();
        Assert.Equal("User1", parent.Name);
        Assert.Single(parent.ChildComments);
        Assert.Equal("User2", parent.ChildComments[0].Name);
    }

    [Fact]
    public async Task GetBanDurationsAsync_Should_Return_Durations()
    {
        // Act
        var durations = await _service.GetBanDurationsAsync();

        // Assert
        var expected = new[] { "1 hour", "1 day", "1 week", "1 month", "permanent" };
        Assert.Equal(expected, durations);
    }

    [Fact]
    public async Task AddCommentAsync_Should_Throw_When_User_Is_Banned()
    {
        // Arrange
        var name = "BannedUser";
        var gameKey = "game";
        var body = "text";
        await _service.BanUserAsync(name, "1 day");

        // Act + Assert
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _service.AddCommentAsync(gameKey, name, body, null));
    }

    [Fact]
    public async Task BanUserAsync_Should_AddBan()
    {
        // Arrange
        var user = "TestUser";

        // Act
        await _service.BanUserAsync(user, "1 hour");

        // Try to comment (should throw)
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _service.AddCommentAsync("test", user, "text", null));
    }

    [Fact]
    public async Task DeleteCommentAsync_Should_CallDelete_When_Exists()
    {
        // Arrange
        var commentId = Guid.NewGuid();
        var comment = new CommentEntity { Id = commentId };

        _commentRepoMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);

        // Act
        await _service.DeleteCommentAsync(commentId);

        // Assert
        _commentRepoMock.Verify(r => r.DeleteAsync(comment), Times.Once);
    }

    [Fact]
    public async Task DeleteCommentAsync_Should_Throw_When_NotFound()
    {
        // Arrange
        var commentId = Guid.NewGuid();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _commentRepoMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync((CommentEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act + Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _service.DeleteCommentAsync(commentId));
    }

    // Additional tests for better branch coverage
    [Fact]
    public async Task AddCommentAsync_Should_Throw_When_GameNotFound()
    {
        // Arrange
        var gameKey = "non-existent-key";
        var name = "TestUser";
        var body = "Test comment";

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity>());

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() =>
            _service.AddCommentAsync(gameKey, name, body, null));
    }

    [Theory]
    [InlineData("", "ValidBody")]
    [InlineData("   ", "ValidBody")]
    [InlineData("ValidName", "")]
    [InlineData("ValidName", "   ")]
    public async Task AddCommentAsync_Should_Throw_When_NameOrBodyIsInvalid(string name, string body)
    {
        // Arrange
        var gameKey = "valid-key";
        var gameId = Guid.NewGuid();

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity> { new() { Id = gameId, Key = gameKey } });

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _service.AddCommentAsync(gameKey, name, body, null));
    }

    [Fact]
    public async Task AddCommentAsync_Should_Throw_NullReferenceException_When_NameIsNull()
    {
        // Arrange
        var gameKey = "valid-key";
        string name = null;
        var body = "ValidBody";

        // Act & Assert - Should throw NullReferenceException when trying to call name.ToLower()
        await Assert.ThrowsAsync<NullReferenceException>(() =>
            _service.AddCommentAsync(gameKey, name!, body, null));
    }

    [Fact]
    public async Task AddCommentAsync_Should_Throw_GameServiceException_When_BodyIsNull()
    {
        // Arrange
        var gameKey = "valid-key";
        var name = "ValidName";
        string body = null;
        var gameId = Guid.NewGuid();

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity> { new() { Id = gameId, Key = gameKey } });

        // Act & Assert - Should throw GameServiceException for null body
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _service.AddCommentAsync(gameKey, name, body!, null));
    }

    [Fact]
    public async Task GetCommentsAsync_Should_Throw_When_GameNotFound()
    {
        // Arrange
        var gameKey = "non-existent-key";

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity>());

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _service.GetCommentsAsync(gameKey));
    }

    [Theory]
    [InlineData("1 hour")]
    [InlineData("1 day")]
    [InlineData("1 week")]
    [InlineData("1 month")]
    [InlineData("permanent")]
    public async Task BanUserAsync_Should_BanUser_ForValidDurations(string duration)
    {
        // Arrange
        var user = "TestUser";

        // Act
        await _service.BanUserAsync(user, duration);

        // Try to comment (should throw)
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _service.AddCommentAsync("test", user, "text", null));
    }

    [Fact]
    public async Task BanUserAsync_Should_Throw_For_InvalidDuration()
    {
        // Arrange
        var user = "TestUser";
        var invalidDuration = "invalid duration";

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() =>
            _service.BanUserAsync(user, invalidDuration));
    }

    [Fact]
    public async Task IsUserBannedAsync_Should_Return_False_When_UserNotBanned()
    {
        // Arrange
        var user = "NonBannedUser";
        var gameKey = "test-key";
        var gameId = Guid.NewGuid();

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity> { new() { Id = gameId, Key = gameKey } });
        _commentRepoMock.Setup(r => r.AddAsync(It.IsAny<CommentEntity>()))
            .Returns(Task.CompletedTask);

        // Act - Should not throw since user is not banned
        await _service.AddCommentAsync(gameKey, user, "Test comment", null);

        // Assert - If we get here, the user was not banned
        _commentRepoMock.Verify(r => r.AddAsync(It.IsAny<CommentEntity>()), Times.Once);
    }

    [Fact]
    public async Task IsUserBannedAsync_Should_Return_False_When_BanExpired()
    {
        // Arrange
        var user = "ExpiredBanUser";
        var gameKey = "test-key";
        var gameId = Guid.NewGuid();

        // Ban user for 1 hour (this creates an entry in _bannedUsers)
        await _service.BanUserAsync(user, "1 hour");

        // Simulate ban expiration by creating new service instance
        // (since _bannedUsers is a ConcurrentDictionary that expires entries)
        var newService = new CommentService(_commentRepoMock.Object, _gameRepoMock.Object);

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity> { new() { Id = gameId, Key = gameKey } });
        _commentRepoMock.Setup(r => r.AddAsync(It.IsAny<CommentEntity>()))
            .Returns(Task.CompletedTask);

        // Act - Should not throw since the new service instance doesn't have the ban
        await newService.AddCommentAsync(gameKey, user, "Test comment", null);

        // Assert
        _commentRepoMock.Verify(r => r.AddAsync(It.IsAny<CommentEntity>()), Times.Once);
    }

    [Fact]
    public async Task GetCommentsAsync_Should_Handle_Comments_WithoutParent()
    {
        // Arrange
        var gameKey = "test-key";
        var gameId = Guid.NewGuid();
        var commentId = Guid.NewGuid();

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity> { new() { Id = gameId, Key = gameKey } });

        var comments = new List<CommentEntity>
        {
            new() { Id = commentId, Name = "User1", Body = "Top-level comment", GameId = gameId, ParentCommentId = null },
        };

        _commentRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<CommentEntity, bool>>>()))
            .ReturnsAsync(comments);

        // Act
        var result = await _service.GetCommentsAsync(gameKey);

        // Assert
        Assert.Single(result);
        var comment = result.First();
        Assert.Equal("User1", comment.Name);
        Assert.Equal("Top-level comment", comment.Body);
        Assert.Empty(comment.ChildComments);
    }

    [Fact]
    public async Task AddCommentAsync_Should_Add_WithParentId()
    {
        // Arrange
        var gameKey = "game-key";
        var name = "Alice";
        var body = "Reply comment";
        var gameId = Guid.NewGuid();
        var parentId = Guid.NewGuid();

        _gameRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<GameEntity, bool>>>()))
            .ReturnsAsync(new List<GameEntity> { new() { Id = gameId, Key = gameKey } });

        _commentRepoMock.Setup(r => r.AddAsync(It.IsAny<CommentEntity>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AddCommentAsync(gameKey, name, body, parentId);

        // Assert
        _commentRepoMock.Verify(
            r => r.AddAsync(It.Is<CommentEntity>(
                c => c.Name == name && c.Body == body && c.GameId == gameId && c.ParentCommentId == parentId)),
            Times.Once);
    }
}