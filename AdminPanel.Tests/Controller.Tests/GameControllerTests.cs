using System.Security.Claims;
using System.Text;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Entity;
using AdminPanel.Web.Controllers;
using AdminPanel.Web.DtoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;

public class GameControllerTests : IDisposable
{
    private readonly Mock<IGameService> _mockGameService;
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly GameController _controller;

    public GameControllerTests()
    {
        _mockGameService = new Mock<IGameService>();
        _mockOrderService = new Mock<IOrderService>();
        var mockLocalizationService = new Mock<IGameLocalizationService>();
        _controller = new GameController(_mockGameService.Object, _mockOrderService.Object, mockLocalizationService.Object);
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        _controller.Dispose();
    }

    [Fact]
    public async Task GetGameByKey_WhenGameExists_ReturnsOkResult()
    {
        // Arrange
        var key = "test-game";
        var game = new GameEntity { Id = Guid.NewGuid(), Name = "Test Game", Key = key };
        _mockGameService.Setup(s => s.GetGameByKeyAsync(key)).ReturnsAsync(game);

        // Act
        var result = await _controller.GetGameByKey(key);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGame = Assert.IsType<GameEntity>(okResult.Value);
        Assert.Equal(game.Key, returnedGame.Key);
    }

    [Fact]
    public async Task Create_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var uiRequest = new UIRequestFormat
        {
            Game = new GameDetails
            {
                Name = "New Game",
                Key = "new-game",
                Description = "Test Description",
            },
            Genres = new List<string> { Guid.NewGuid().ToString() },
            Platforms = new List<string> { Guid.NewGuid().ToString() },
            Publisher = "Test Publisher",
        };

        _mockGameService.Setup(s => s.AddGameAsync(It.IsAny<GameDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(uiRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Game created successfully.", okResult.Value);
    }

    [Fact]
    public async Task DeleteGameByKey_WhenGameExists_ReturnsOkResult()
    {
        // Arrange
        var key = "test-game";
        _mockGameService.Setup(s => s.DeleteGameAsync(key))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteGameByKey(key);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Deleted successfully", okResult.Value);
    }

    [Fact]
    public async Task DeleteGameByKey_WhenGameDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var key = "non-existent-game";
        _mockGameService.Setup(s => s.DeleteGameAsync(key))
            .ThrowsAsync(new GameNotFoundException("Game not found"));

        // Act
        var result = await _controller.DeleteGameByKey(key);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task GetGamesByGenre_WithValidGenreId_ReturnsOkResult()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var expectedGames = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Test Game 1" },
            new() { Id = Guid.NewGuid(), Name = "Test Game 2" },
        };
        _mockGameService.Setup(s => s.GetGamesByGenreAsync(genreId))
            .ReturnsAsync(expectedGames);

        // Act
        var result = await _controller.GetGamesByGenre(genreId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameEntity>>(okResult.Value);
        Assert.Equal(expectedGames.Count, returnedGames.Count());
    }

    [Fact]
    public async Task GetGamesByPlatform_WithValidPlatformId_ReturnsOkResult()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var expectedGames = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Test Game 1" },
            new() { Id = Guid.NewGuid(), Name = "Test Game 2" },
        };
        _mockGameService.Setup(s => s.GetGamesByPlatformAsync(platformId))
            .ReturnsAsync(expectedGames);

        // Act
        var result = await _controller.GetGamesByPlatform(platformId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameEntity>>(okResult.Value);
        Assert.Equal(expectedGames.Count, returnedGames.Count());
    }

    [Fact]
    public async Task DownloadGameFile_WhenGameExists_ReturnsFileResult()
    {
        // Arrange
        var key = "test-game";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("test content"));
        _mockGameService.Setup(s => s.GetGameFileAsync(key))
            .ReturnsAsync(stream);

        // Act
        var result = await _controller.DownloadGameFile(key);

        // Assert
        var fileResult = Assert.IsType<FileStreamResult>(result);
        Assert.Equal("text/plain", fileResult.ContentType);
        Assert.Contains(key, fileResult.FileDownloadName);
    }

    [Fact]
    public async Task DownloadGameFile_WhenGameDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var key = "non-existent-game";
        _mockGameService.Setup(s => s.GetGameFileAsync(key))
            .ThrowsAsync(new GameNotFoundException("Game not found"));

        // Act
        var result = await _controller.DownloadGameFile(key);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task AddGameToCart_WithValidKey_ReturnsOkResult()
    {
        // Arrange
        var key = Guid.NewGuid();
        _mockOrderService.Setup(s => s.AddGameToCartAsync(key))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddGameToCart(key);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task AddGameToCart_WithInvalidKey_ReturnsBadRequest()
    {
        // Arrange
        var key = Guid.NewGuid();
        _mockOrderService.Setup(s => s.AddGameToCartAsync(key))
            .ThrowsAsync(new Exception("Invalid game"));

        // Act
        var result = await _controller.AddGameToCart(key);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid game", badRequestResult.Value);
    }

    // Additional comprehensive tests
    [Fact]
    public async Task GetGames_WithValidFilter_ReturnsOkWithFilteredGames()
    {
        // Arrange
        SetupAuthenticatedUser(); // Setup authentication for this test

        var filter = new GameFilterDto
        {
            Name = "Test",
            GenreIds = new List<Guid> { Guid.NewGuid() },
            PlatformIds = new List<Guid> { Guid.NewGuid() },
            MinPrice = 10,
            MaxPrice = 50,
            Page = 1,
            PageCount = 10,
        };

        var gameResult = new PagedGamesResultDto
        {
            Games = new List<GameResponseDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Test Game 1", Key = "test-game-1" },
                new() { Id = Guid.NewGuid(), Name = "Test Game 2", Key = "test-game-2" },
            },
            TotalCount = 2,
        };

        _mockGameService.Setup(s => s.GetFilteredGamesAsync(It.IsAny<GameFilterDto>()))
                        .ReturnsAsync(gameResult);

        // Act
        var result = await _controller.GetGames(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<PagedGamesResultDto>(okResult.Value);
        Assert.Equal(2, returnedResult.Games.Count());
        Assert.Equal(2, returnedResult.TotalCount);
    }

    [Fact]
    public async Task GetGames_WhenServiceThrowsException_ReturnsBadRequest()
    {
        // Arrange
        SetupAuthenticatedUser(); // Setup authentication for this test

        var filter = new GameFilterDto();
        _mockGameService.Setup(s => s.GetFilteredGamesAsync(It.IsAny<GameFilterDto>()))
                        .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _controller.GetGames(filter);

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Contains("An error occurred while retrieving games", statusResult.Value.ToString());
    }

    [Fact]
    public async Task GetAllGames_ReturnsOkWithAllGames()
    {
        // Arrange
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game 1", Key = "game-1" },
            new() { Id = Guid.NewGuid(), Name = "Game 2", Key = "game-2" },
            new() { Id = Guid.NewGuid(), Name = "Game 3", Key = "game-3" },
        };

        _mockGameService.Setup(s => s.GetAllGamesAsync()).ReturnsAsync(games);

        // Act
        var result = await _controller.GetAllGames();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameResponseDto>>(okResult.Value);
        Assert.Equal(3, returnedGames.Count());
    }

    [Fact]
    public async Task GetAllGames_WhenServiceThrowsException_ReturnsBadRequest()
    {
        // Arrange
        _mockGameService.Setup(s => s.GetAllGamesAsync())
                        .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _controller.GetAllGames();

        // Assert
        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Contains("An error occurred while retrieving all games", statusResult.Value.ToString());
    }

    [Fact]
    public async Task GetGameByKey_WhenServiceThrowsGeneralException_ThrowsException()
    {
        // Arrange
        var key = "test-game";
        _mockGameService.Setup(s => s.GetGameByKeyAsync(key))
                        .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetGameByKey(key));
    }

    [Fact]
    public async Task Create_WithInvalidGenres_ReturnsBadRequest()
    {
        // Arrange
        var uiRequest = new UIRequestFormat
        {
            Game = new GameDetails
            {
                Name = "New Game",
                Key = "new-game",
                Description = "Test Description",
            },
            Genres = new List<string> { "invalid-genre-id" },
            Platforms = new List<string> { Guid.NewGuid().ToString() },
            Publisher = "Test Publisher",
        };

        _mockGameService.Setup(s => s.AddGameAsync(It.IsAny<GameDto>()))
                        .ThrowsAsync(new InvalidGenresException(new List<Guid> { Guid.NewGuid() }, "Invalid genre"));

        // Act
        var result = await _controller.Create(uiRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Invalid genre", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task Create_WhenGameAlreadyExists_ReturnsConflict()
    {
        // Arrange
        var uiRequest = new UIRequestFormat
        {
            Game = new GameDetails
            {
                Name = "Existing Game",
                Key = "existing-game",
                Description = "Test Description",
            },
            Genres = new List<string> { Guid.NewGuid().ToString() },
            Platforms = new List<string> { Guid.NewGuid().ToString() },
            Publisher = "Test Publisher",
        };

        _mockGameService.Setup(s => s.AddGameAsync(It.IsAny<GameDto>()))
                        .ThrowsAsync(new GameAlreadyExistsException("existing-game", "Game already exists"));

        // Act
        var result = await _controller.Create(uiRequest);

        // Assert
        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Contains("Game already exists", conflictResult.Value.ToString());
    }

    [Fact]
    public async Task Update_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var key = "test-game";
        var uiRequest = new UIRequestFormat
        {
            Game = new GameDetails
            {
                Name = "Updated Game",
                Key = key,
                Description = "Updated Description",
            },
            Genres = new List<string> { Guid.NewGuid().ToString() },
            Platforms = new List<string> { Guid.NewGuid().ToString() },
            Publisher = "Updated Publisher",
        };

        _mockGameService.Setup(s => s.UpdateGameAsync(key, It.IsAny<GameDto>()))
                        .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateGame(key, uiRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Game updated successfully.", okResult.Value);
    }

    [Fact]
    public async Task Update_WhenGameNotFound_ReturnsNotFound()
    {
        // Arrange
        var key = "non-existent-game";
        var uiRequest = new UIRequestFormat
        {
            Game = new GameDetails
            {
                Name = "Updated Game",
                Key = key,
                Description = "Updated Description",
            },
            Genres = new List<string> { Guid.NewGuid().ToString() },
            Platforms = new List<string> { Guid.NewGuid().ToString() },
            Publisher = "Updated Publisher",
        };

        _mockGameService.Setup(s => s.UpdateGameAsync(key, It.IsAny<GameDto>()))
                        .ThrowsAsync(new GameNotFoundException("Game not found"));

        // Act
        var result = await _controller.UpdateGame(key, uiRequest);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task GetGamesByGenre_WhenServiceThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        _mockGameService.Setup(s => s.GetGamesByGenreAsync(genreId))
                        .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _controller.GetGamesByGenre(genreId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Database error", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task GetGamesByPlatform_WhenServiceThrowsException_ReturnsBadRequest()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        _mockGameService.Setup(s => s.GetGamesByPlatformAsync(platformId))
                        .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act
        var result = await _controller.GetGamesByPlatform(platformId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Database error", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task DownloadGameFile_WhenServiceThrowsGeneralException_ReturnsNotFound()
    {
        // Arrange
        var key = "test-game";
        _mockGameService.Setup(s => s.GetGameFileAsync(key))
                        .ThrowsAsync(new InvalidOperationException("File access error"));

        // Act
        var result = await _controller.DownloadGameFile(key);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("File access error", notFoundResult.Value.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetGameByKey_WithInvalidKey_CallsServiceWithInvalidKey(string key)
    {
        // Arrange
        _mockGameService.Setup(s => s.GetGameByKeyAsync(key))
                        .ThrowsAsync(new GameNotFoundException("Game not found"));

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _controller.GetGameByKey(key));
        _mockGameService.Verify(s => s.GetGameByKeyAsync(key), Times.Once);
    }

    [Fact]
    public async Task GetGamesByGenre_WithEmptyGuid_CallsService()
    {
        // Arrange
        var genreId = Guid.Empty;
        var emptyGames = new List<GameEntity>();
        _mockGameService.Setup(s => s.GetGamesByGenreAsync(genreId))
                        .ReturnsAsync(emptyGames);

        // Act
        var result = await _controller.GetGamesByGenre(genreId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameEntity>>(okResult.Value);
        Assert.Empty(returnedGames);
        _mockGameService.Verify(s => s.GetGamesByGenreAsync(genreId), Times.Once);
    }

    [Fact]
    public async Task GetGamesByPlatform_WithEmptyGuid_CallsService()
    {
        // Arrange
        var platformId = Guid.Empty;
        var emptyGames = new List<GameEntity>();
        _mockGameService.Setup(s => s.GetGamesByPlatformAsync(platformId))
                        .ReturnsAsync(emptyGames);

        // Act
        var result = await _controller.GetGamesByPlatform(platformId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameEntity>>(okResult.Value);
        Assert.Empty(returnedGames);
        _mockGameService.Verify(s => s.GetGamesByPlatformAsync(platformId), Times.Once);
    }

    private void SetupAuthenticatedUser()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "testuser"),
            new(ClaimTypes.Email, "test@example.com"),
            new(ClaimTypes.NameIdentifier, "user-123"),
        };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);

        var context = new DefaultHttpContext
        {
            User = principal,
        };

        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = context,
        };
    }
}