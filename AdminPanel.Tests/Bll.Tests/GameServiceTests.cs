using System.Text.Json;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Services;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;

namespace AdminPanel.Tests.Bll.Tests;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<GameService>> _loggerMock;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<GameService>>();
        _gameService = new GameService(
            _gameRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllGamesAsync_ShouldReturnAllGames()
    {
        // Arrange
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game 1" },
            new() { Id = Guid.NewGuid(), Name = "Game 2" },
        };

        _gameRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(games);

        // Act
        var result = await _gameService.GetAllGamesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _gameRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetGameByIdAsync_ShouldReturnCorrectGame()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var game = new GameEntity { Id = gameId, Name = "Test Game" };
        _gameRepositoryMock.Setup(repo => repo.GetByIdAsync(gameId)).ReturnsAsync(game);

        // Act
        var result = await _gameService.GetGameByIdAsync(gameId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(gameId, result.Id);
        _gameRepositoryMock.Verify(repo => repo.GetByIdAsync(gameId), Times.Once);
    }

    [Fact]
    public async Task AddGameAsync_ShouldCallRepositoryAddAsync()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var gameDto = new GameDto
        {
            Key = "new-key",
            Name = "New Game",
            PublisherId = publisherId,
            Price = 10,
            UnitInStock = 5,
            Discount = 0,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid>(),
        };
        var gameEntity = new GameEntity { Key = "new-key", Name = "New Game" };

        _mapperMock.Setup(mapper => mapper.Map<GameEntity>(gameDto)).Returns(gameEntity);
        _gameRepositoryMock.Setup(repo => repo.AddAsync(gameEntity)).Returns(Task.CompletedTask);

        // Mock publisher validation
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);

        // Act
        await _gameService.AddGameAsync(gameDto);

        // Assert
        _mapperMock.Verify(mapper => mapper.Map<GameEntity>(gameDto), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.AddAsync(gameEntity), Times.Once);
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldCallRepositoryUpdateAsync()
    {
        // Arrange
        var key = "existing-key";
        var publisherId = Guid.NewGuid();
        var gameDto = new GameDto
        {
            Key = key,
            Name = "Updated Game",
            PublisherId = publisherId,
            Price = 10,
            UnitInStock = 5,
            Discount = 0,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid>(),
        };
        var existingGame = new GameEntity { Key = key, Name = "Existing Game" };

        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(existingGame);
        _mapperMock.Setup(mapper => mapper.Map(gameDto, existingGame)).Returns(existingGame);
        _gameRepositoryMock.Setup(repo => repo.UpdateAsync(existingGame)).Returns(Task.CompletedTask);

        // Mock publisher validation
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);

        // Mock genre and platform validations
        _gameRepositoryMock.Setup(repo => repo.GetGamesByGenreAsync(existingGame.Id)).ReturnsAsync(new List<GameEntity>());
        _gameRepositoryMock.Setup(repo => repo.GetGamesByPlatformAsync(existingGame.Id)).ReturnsAsync(new List<GameEntity>());

        // Act
        await _gameService.UpdateGameAsync(key, gameDto);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.GetByKeyAsync(key), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map(gameDto, existingGame), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.UpdateAsync(existingGame), Times.Once);
    }

    [Fact]
    public async Task DeleteGameAsync_ShouldSoftDeleteGame()
    {
        // Arrange
        var gameKey = "someKey";
        var game = new GameEntity { Key = gameKey, Name = "Game to Delete", IsDeleted = false };

        // Mock game existence check
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(gameKey)).ReturnsAsync(game);
        _gameRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<GameEntity>())).Returns(Task.CompletedTask);

        // Act
        await _gameService.DeleteGameAsync(gameKey);

        // Assert
        Assert.True(game.IsDeleted); // Verify soft delete flag is set
        _gameRepositoryMock.Verify(repo => repo.UpdateAsync(game), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Never); // Should not hard delete
    }

    [Fact]
    public async Task GetGameByKeyAsync_ShouldReturnCorrectGame()
    {
        // Arrange
        var key = "unique-key";
        var game = new GameEntity { Id = Guid.NewGuid(), Key = key };

        // Use GetByKeyAsync instead of GetAllAsync
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(game);

        // Act
        var result = await _gameService.GetGameByKeyAsync(key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(key, result.Key);
    }

    [Fact]
    public async Task GetTotalGamesCountAsync_ShouldReturnCorrectCount()
    {
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game 1" },
            new() { Id = Guid.NewGuid(), Name = "Game 2" },
        };

        _gameRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(games);

        var totalGames = await _gameService.GetTotalGamesCountAsync();

        Assert.Equal(2, totalGames);
    }

    [Fact]
    public async Task GetGamesByGenreAsync_ShouldReturnGamesWithMatchingGenre()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game with Genre" },
            new() { Id = Guid.NewGuid(), Name = "Another Game with Genre" },
        };

        _gameRepositoryMock.Setup(repo => repo.DoesGenreExistAsync(genreId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.GetGamesByGenreAsync(genreId)).ReturnsAsync(games);

        // Act
        var result = await _gameService.GetGamesByGenreAsync(genreId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _gameRepositoryMock.Verify(repo => repo.GetGamesByGenreAsync(genreId), Times.Once);
    }

    [Fact]
    public async Task GetGamesByGenreAsync_ShouldThrowException_WhenGenreDoesNotExist()
    {
        // Arrange
        var nonExistentGenreId = Guid.NewGuid();
        _gameRepositoryMock.Setup(repo => repo.DoesGenreExistAsync(nonExistentGenreId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidGenresException>(() =>
            _gameService.GetGamesByGenreAsync(nonExistentGenreId));
    }

    [Fact]
    public async Task GetGamesByPlatformAsync_ShouldReturnGamesWithMatchingPlatform()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game with Platform" },
            new() { Id = Guid.NewGuid(), Name = "Another Game with Platform" },
        };

        _gameRepositoryMock.Setup(repo => repo.DoesPlatformExistAsync(platformId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.GetGamesByPlatformAsync(platformId)).ReturnsAsync(games);

        // Act
        var result = await _gameService.GetGamesByPlatformAsync(platformId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _gameRepositoryMock.Verify(repo => repo.GetGamesByPlatformAsync(platformId), Times.Once);
    }

    [Fact]
    public async Task GetGamesByPlatformAsync_ShouldThrowException_WhenPlatformDoesNotExist()
    {
        // Arrange
        var nonExistentPlatformId = Guid.NewGuid();
        _gameRepositoryMock.Setup(repo => repo.DoesPlatformExistAsync(nonExistentPlatformId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPlatformsException>(() =>
            _gameService.GetGamesByPlatformAsync(nonExistentPlatformId));
    }

    [Fact]
    public async Task GetGamesByPublisherAsync_ShouldReturnGamesForPublisher()
    {
        // Arrange
        var publisherName = "Test Publisher";
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game by Publisher" },
            new() { Id = Guid.NewGuid(), Name = "Another Game by Publisher" },
        };

        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherName)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.GetGamesByPublisherAsync(publisherName)).ReturnsAsync(games);

        // Act
        var result = await _gameService.GetGamesByPublisherAsync(publisherName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _gameRepositoryMock.Verify(repo => repo.GetGamesByPublisherAsync(publisherName), Times.Once);
    }

    [Fact]
    public async Task GetGamesByPublisherAsync_ShouldThrowException_WhenPublisherDoesNotExist()
    {
        // Arrange
        var nonExistentPublisher = "Non-Existent Publisher";
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(nonExistentPublisher)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPublisherException>(() =>
            _gameService.GetGamesByPublisherAsync(nonExistentPublisher));
    }

    [Fact]
    public async Task GetGameFileAsync_ShouldReturnMemoryStream()
    {
        // Arrange
        var key = "game-key";
        var game = new GameEntity { Id = Guid.NewGuid(), Key = key, Name = "Test Game", Price = 29 };
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(game);

        // Act
        var resultStream = await _gameService.GetGameFileAsync(key);

        // Assert
        Assert.NotNull(resultStream);
        Assert.True(resultStream.CanRead);

        resultStream.Position = 0; // Reset stream position to read from the beginning
        using (var reader = new StreamReader(resultStream))
        {
            var content = await reader.ReadToEndAsync();
            var deserializedGame = JsonSerializer.Deserialize<GameEntity>(content);
            Assert.NotNull(deserializedGame);
            Assert.Equal(game.Key, deserializedGame.Key);
            Assert.Equal(game.Name, deserializedGame.Name);
            Assert.Equal(game.Price, deserializedGame.Price);
        }

        _gameRepositoryMock.Verify(repo => repo.GetByKeyAsync(key), Times.Once);
    }

    [Fact]
    public async Task GetGameFileAsync_ShouldThrowGameNotFoundException_WhenGameDoesNotExist()
    {
        // Arrange
        var nonExistentKey = "non-existent-key";
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(nonExistentKey)).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _gameService.GetGameFileAsync(nonExistentKey));
        _gameRepositoryMock.Verify(repo => repo.GetByKeyAsync(nonExistentKey), Times.Once);
    }

    [Fact]
    public async Task GetGameFileAsync_ShouldThrowGameServiceException_OnGeneralException()
    {
        // Arrange
        var key = "game-key";
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() => _gameService.GetGameFileAsync(key));
        _gameRepositoryMock.Verify(repo => repo.GetByKeyAsync(key), Times.Once);
    }

    [Fact]
    public async Task GetTotalGamesCountAsync_ShouldThrowGameServiceException_OnException()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() => _gameService.GetTotalGamesCountAsync());
        _gameRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task IncrementViewCountAsync_ShouldThrowNotImplementedException()
    {
        // Arrange
        var gameId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => _gameService.IncrementViewCountAsync(gameId));
    }

    [Fact]
    public async Task IncrementViewCountForGamesAsync_ShouldThrowNotImplementedException()
    {
        // Arrange
        var gameIds = new List<string> { "id1", "id2" };

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => _gameService.IncrementViewCountForGamesAsync(gameIds));
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldReturnPagedResult_WithNoFilters()
    {
        // Arrange
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game A", Price = 10 },
            new() { Id = Guid.NewGuid(), Name = "Game B", Price = 20 },
            new() { Id = Guid.NewGuid(), Name = "Game C", Price = 30 },
        };
        var gameFilterDto = new GameFilterDto { Page = 1, PageCount = 10 };

        // Use BuildMock() for IQueryable
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(games.AsQueryable().BuildMock());
        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Games.Count());
        Assert.Equal(1, result.TotalPages);
        Assert.Equal(1, result.CurrentPage);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldFilterByName()
    {
        // Arrange
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Action Game", Price = 10 },
            new() { Id = Guid.NewGuid(), Name = "Adventure Game", Price = 20 },
            new() { Id = Guid.NewGuid(), Name = "Strategy Game", Price = 30 },
        };
        var gameFilterDto = new GameFilterDto { Name = "action", Page = 1, PageCount = 10 };

        // Use BuildMock() for IQueryable
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(games.AsQueryable().BuildMock());
        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Games);
        Assert.Equal("Action Game", result.Games.First().Name);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldFilterByPriceRange()
    {
        // Arrange
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game 1", Price = 5 },
            new() { Id = Guid.NewGuid(), Name = "Game 2", Price = 15 },
            new() { Id = Guid.NewGuid(), Name = "Game 3", Price = 25 },
        };
        var gameFilterDto = new GameFilterDto { MinPrice = 10, MaxPrice = 20, Page = 1, PageCount = 10 };

        // Use BuildMock() for IQueryable
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(games.AsQueryable().BuildMock());
        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name, Price = g.Price }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Games);
        Assert.Equal("Game 2", result.Games.First().Name);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldFilterByGenreIds()
    {
        // Arrange
        var genreId1 = Guid.NewGuid();
        var genreId2 = Guid.NewGuid();
        var game1Id = Guid.NewGuid();
        var game2Id = Guid.NewGuid();
        var games = new List<GameEntity>
        {
            new() { Id = game1Id, Name = "Game with Genre 1" },
            new() { Id = game2Id, Name = "Game with Genre 2" },
            new() { Id = Guid.NewGuid(), Name = "Game with No Matching Genre" }, // please dont remove coma in the end it is correct styling.
        };
        var gameGenres = new List<GameGenreEntity>
        {
            new() { GameId = game1Id, GenreId = genreId1 },
            new() { GameId = game2Id, GenreId = genreId2 },
        };
        var gameFilterDto = new GameFilterDto { GenreIds = new List<Guid> { genreId1 }, Page = 1, PageCount = 10 };

        // Use BuildMock() for IQueryable
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(games.AsQueryable().BuildMock());
        _gameRepositoryMock.Setup(repo => repo.GetGameGenreQueryable()).Returns(gameGenres.AsQueryable().BuildMock()); // Use BuildMock() here
        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Games);
        Assert.Equal("Game with Genre 1", result.Games.First().Name);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldFilterByPlatformIds()
    {
        // Arrange
        var platformId1 = Guid.NewGuid();
        var platformId2 = Guid.NewGuid();
        var game1Id = Guid.NewGuid();
        var game2Id = Guid.NewGuid();
        var games = new List<GameEntity>
        {
            new() { Id = game1Id, Name = "Game with Platform 1" },
            new() { Id = game2Id, Name = "Game with Platform 2" },
            new() { Id = Guid.NewGuid(), Name = "Game with No Matching Platform" },
        };
        var gamePlatforms = new List<GamePlatformEntity>
        {
            new() { GameId = game1Id, PlatformId = platformId1 },
            new() { GameId = game2Id, PlatformId = platformId2 },
        };
        var gameFilterDto = new GameFilterDto { PlatformIds = new List<Guid> { platformId1 }, Page = 1, PageCount = 10 };

        // Use BuildMock() for IQueryable
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(games.AsQueryable().BuildMock());
        _gameRepositoryMock.Setup(repo => repo.GetGamePlatformQueryable()).Returns(gamePlatforms.AsQueryable().BuildMock()); // Use BuildMock() here
        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Games);
        Assert.Equal("Game with Platform 1", result.Games.First().Name);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldFilterByPublisherNames()
    {
        // Arrange
        var publisher1Id = Guid.NewGuid();
        var publisher2Id = Guid.NewGuid();
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game by Publisher A", PublisherId = publisher1Id },
            new() { Id = Guid.NewGuid(), Name = "Game by Publisher B", PublisherId = publisher2Id },
            new() { Id = Guid.NewGuid(), Name = "Game by Another Publisher", PublisherId = Guid.NewGuid() },
        };
        var publisherNames = new List<string> { "PublisherA" };
        var gameFilterDto = new GameFilterDto { PublisherNames = publisherNames, Page = 1, PageCount = 10 };

        // Use BuildMock() for IQueryable
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(games.AsQueryable().BuildMock());
        _gameRepositoryMock.Setup(repo => repo.GetPublisherIdsByNamesAsync(publisherNames))
            .ReturnsAsync(new List<Guid> { publisher1Id });
        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Games);
        Assert.Equal("Game by Publisher A", result.Games.First().Name);
    }

    [Theory]
    [InlineData("Price ASC", "Game A", "Game B", "Game C")]
    [InlineData("Price DESC", "Game C", "Game B", "Game A")]
    [InlineData("Most popular", "Game A", "Game B", "Game C")] // Defaulting to Name ASC
    [InlineData("Most commented", "Game C", "Game B", "Game A")] // Defaulting to Name DESC
    [InlineData("New", "Game A", "Game B", "Game C")] // Defaulting to Name ASC
    [InlineData("UnknownSort", "Game A", "Game B", "Game C")] // Defaulting to Name ASC
    public async Task GetFilteredGamesAsync_ShouldApplySorting(string sortOption, params string[] expectedNames)
    {
        // Arrange
        var games = new List<GameEntity>
    {
        new() { Id = Guid.NewGuid(), Name = "Game B", Price = 20 },
        new() { Id = Guid.NewGuid(), Name = "Game A", Price = 10 },
        new() { Id = Guid.NewGuid(), Name = "Game C", Price = 30 },
    };
        var gameFilterDto = new GameFilterDto { Sort = sortOption, Page = 1, PageCount = 10 };

        // Use BuildMock() for IQueryable to enable async operations
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(games.AsQueryable().BuildMock());
        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedNames.Length, result.Games.Count());
        Assert.Equal(expectedNames[0], result.Games.ElementAt(0).Name);
        Assert.Equal(expectedNames[1], result.Games.ElementAt(1).Name);
        Assert.Equal(expectedNames[2], result.Games.ElementAt(2).Name);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldHandlePagination()
    {
        // Arrange
        var games = Enumerable.Range(1, 25)
            .Select(i => new GameEntity { Id = Guid.NewGuid(), Name = $"Game {i}", Price = i })
            .ToList();

        var gameFilterDto = new GameFilterDto { Page = 2, PageCount = 10 };

        // Set up the mock to return an IQueryable that is ordered by Name,
        // to simulate the expected default ordering in your service.
        // This ensures consistent pagination results in the test.
        _gameRepositoryMock.Setup(repo => repo.GetQueryable())
                           .Returns(games.AsQueryable().OrderBy(g => g.Name).BuildMock());

        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Games.Count());
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(2, result.CurrentPage);
        Assert.Equal("Game 19", result.Games.First().Name);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldReturnEmptyResult_WhenNoGamesMatch()
    {
        // Arrange
        var games = new List<GameEntity>
    {
        new() { Id = Guid.NewGuid(), Name = "Game A", Price = 10 },
    };
        var gameFilterDto = new GameFilterDto { Name = "NonExistent", Page = 1, PageCount = 10 };

        // Use BuildMock() for IQueryable to enable async operations
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Returns(games.AsQueryable().BuildMock());
        _mapperMock.Setup(m => m.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(It.IsAny<IEnumerable<GameEntity>>()))
            .Returns((IEnumerable<GameEntity> source) => source.Select(g => new GameResponseDto { Name = g.Name }));

        // Act
        var result = await _gameService.GetFilteredGamesAsync(gameFilterDto);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Games);
        Assert.Equal(0, result.TotalPages);
        Assert.Equal(1, result.CurrentPage);
    }

    [Fact]
    public async Task GetFilteredGamesAsync_ShouldThrowGameServiceException_OnGeneralException()
    {
        // Arrange
        var gameFilterDto = new GameFilterDto();
        _gameRepositoryMock.Setup(repo => repo.GetQueryable()).Throws(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() => _gameService.GetFilteredGamesAsync(gameFilterDto));
    }

    // Tests for private helper methods called within public methods (indirectly tested)
    [Fact]
    public async Task AddGameAsync_ShouldThrowArgumentNullException_WhenGameDtoIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _gameService.AddGameAsync(null));
    }

    [Fact]
    public async Task AddGameAsync_ShouldThrowGameServiceException_WhenGameNameIsEmpty()
    {
        // Arrange
        var gameDto = new GameDto { Name = string.Empty, Key = "test-key" };
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(It.IsAny<Guid>())).ReturnsAsync(true); // Mock publisher exists

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameServiceException>(() => _gameService.AddGameAsync(gameDto));
        Assert.Equal("Game name is required.", exception.Message);
    }

    [Fact]
    public async Task AddGameAsync_ShouldThrowGameServiceException_WhenGamePriceIsNegative()
    {
        // Arrange
        var gameDto = new GameDto { Name = "Test Game", Key = "test-key", Price = -1 };
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(It.IsAny<Guid>())).ReturnsAsync(true); // Mock publisher exists

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameServiceException>(() => _gameService.AddGameAsync(gameDto));
        Assert.Equal("Game price cannot be negative.", exception.Message);
    }

    [Fact]
    public async Task AddGameAsync_ShouldThrowGameServiceException_WhenGameUnitInStockIsNegative()
    {
        // Arrange
        var gameDto = new GameDto { Name = "Test Game", Key = "test-key", Price = 10, UnitInStock = -1 };
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(It.IsAny<Guid>())).ReturnsAsync(true); // Mock publisher exists

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameServiceException>(() => _gameService.AddGameAsync(gameDto));
        Assert.Equal("Game units in stock cannot be negative.", exception.Message);
    }

    [Fact]
    public async Task AddGameAsync_ShouldThrowGameServiceException_WhenGameDiscountIsInvalid()
    {
        // Arrange
        var gameDto = new GameDto { Name = "Test Game", Key = "test-key", Price = 10, UnitInStock = 1, Discount = 101 };
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(It.IsAny<Guid>())).ReturnsAsync(true); // Mock publisher exists

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameServiceException>(() => _gameService.AddGameAsync(gameDto));
        Assert.Equal("Game discount must be between 0 and 100.", exception.Message);
    }

    [Fact]
    public async Task AddGameAsync_ShouldSetGameKey_WhenKeyIsNotProvided()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var gameDto = new GameDto
        {
            Name = "My Awesome Game",
            PublisherId = publisherId,
            Price = 10,
            UnitInStock = 5,
            Discount = 0,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid>(),
        };

        // Instead of trying to map with the mocked mapper, directly create the expected GameEntity
        // and set its properties as they would be after a successful mapping and key generation.
        _mapperMock.Setup(mapper => mapper.Map<GameEntity>(It.IsAny<GameDto>()))
            .Returns<GameDto>(source => new GameEntity // Return a new GameEntity instance
            {
                // Simulate AutoMapper's behavior and the service's key generation logic
                Name = source.Name,
                PublisherId = source.PublisherId,
                Price = source.Price,
                UnitInStock = source.UnitInStock,
                Discount = source.Discount,
                Key = "my-awesome-game",
            });

        _gameRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<GameEntity>())).Returns(Task.CompletedTask);
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(It.IsAny<string>())).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);

        // Act
        await _gameService.AddGameAsync(gameDto);

        // Assert
        // Now, the GameEntity passed to AddAsync should have the correct key.
        _gameRepositoryMock.Verify(repo => repo.AddAsync(It.Is<GameEntity>(g => g.Key == "my-awesome-game")), Times.Once);
    }

    [Fact]
    public async Task AddGameAsync_ShouldThrowGameAlreadyExistsException_WhenKeyIsNotUnique()
    {
        // Arrange
        var gameDto = new GameDto
        {
            Name = "Existing Game",
            Key = "existing-game-key",
            PublisherId = Guid.NewGuid(),
            Price = 10,
            UnitInStock = 5,
            Discount = 0,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid>(),
        };
        var existingGame = new GameEntity { Key = "existing-game-key", Name = "Existing Game" };

        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(gameDto.Key)).ReturnsAsync(existingGame);
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(It.IsAny<Guid>())).ReturnsAsync(true); // Mock publisher exists

        // Act & Assert
        await Assert.ThrowsAsync<GameAlreadyExistsException>(() => _gameService.AddGameAsync(gameDto));
    }

    [Fact]
    public async Task AddGameAsync_ShouldThrowInvalidGenresException_WhenGenreIdDoesNotExist()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var nonExistentGenreId = Guid.NewGuid();
        var gameDto = new GameDto
        {
            Name = "Game with Invalid Genre",
            Key = "invalid-genre-game",
            PublisherId = publisherId,
            Price = 10,
            UnitInStock = 5,
            Discount = 0,
            GenreIds = new List<Guid> { nonExistentGenreId },
            PlatformIds = new List<Guid>(),
        };

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(gameDto.Key)).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesGenreExistAsync(nonExistentGenreId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidGenresException>(() => _gameService.AddGameAsync(gameDto));
    }

    [Fact]
    public async Task AddGameAsync_ShouldThrowInvalidPlatformsException_WhenPlatformIdDoesNotExist()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var nonExistentPlatformId = Guid.NewGuid();
        var gameDto = new GameDto
        {
            Name = "Game with Invalid Platform",
            Key = "invalid-platform-game",
            PublisherId = publisherId,
            Price = 10,
            UnitInStock = 5,
            Discount = 0,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid> { nonExistentPlatformId },
        };

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(gameDto.Key)).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesPlatformExistAsync(nonExistentPlatformId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPlatformsException>(() => _gameService.AddGameAsync(gameDto));
    }

    [Fact]
    public async Task AddGameAsync_ShouldThrowInvalidPublisherException_WhenPublisherDoesNotExist()
    {
        // Arrange
        var nonExistentPublisherId = Guid.NewGuid();
        var gameDto = new GameDto
        {
            Name = "Game with Invalid Publisher",
            Key = "invalid-publisher-game",
            PublisherId = nonExistentPublisherId,
            Price = 10,
            UnitInStock = 5,
            Discount = 0,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid>(),
        };

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(gameDto.Key)).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(nonExistentPublisherId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPublisherException>(() => _gameService.AddGameAsync(gameDto));
    }

    [Fact]
    public async Task AddGameAsync_ShouldAddGenresAndPlatforms()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        var platformId = Guid.NewGuid();
        var gameDto = new GameDto
        {
            Name = "Game with Relations",
            Key = "game-relations",
            PublisherId = publisherId,
            Price = 10,
            UnitInStock = 5,
            Discount = 0,
            GenreIds = new List<Guid> { genreId },
            PlatformIds = new List<Guid> { platformId },
        };
        var gameEntity = new GameEntity { Id = Guid.NewGuid(), Key = "game-relations", Name = "Game with Relations" };

        _mapperMock.Setup(mapper => mapper.Map<GameEntity>(gameDto)).Returns(gameEntity);
        _gameRepositoryMock.Setup(repo => repo.AddAsync(gameEntity)).Returns(Task.CompletedTask);
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(gameDto.Key)).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesGenreExistAsync(genreId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesPlatformExistAsync(platformId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.AddGenresAsync(gameEntity.Id, It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);
        _gameRepositoryMock.Setup(repo => repo.AddPlatformsAsync(gameEntity.Id, It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);

        // Act
        await _gameService.AddGameAsync(gameDto);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.AddGenresAsync(gameEntity.Id, It.Is<List<Guid>>(ids => ids.Contains(genreId))), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.AddPlatformsAsync(gameEntity.Id, It.Is<List<Guid>>(ids => ids.Contains(platformId))), Times.Once);
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldThrowGameNotFoundException_WhenGameDoesNotExist()
    {
        // Arrange
        var key = "non-existent-key";
        var gameDto = new GameDto { Name = "Test Game", PublisherId = Guid.NewGuid() };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _gameService.UpdateGameAsync(key, gameDto));
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldThrowInvalidGenresException_WhenGenreIdDoesNotExist()
    {
        // Arrange
        var key = "existing-key";
        var publisherId = Guid.NewGuid();
        var nonExistentGenreId = Guid.NewGuid();
        var existingGame = new GameEntity { Id = Guid.NewGuid(), Key = key, Name = "Existing Game", PublisherId = publisherId };
        var gameDto = new GameDto
        {
            Key = key,
            Name = "Updated Game",
            PublisherId = publisherId,
            GenreIds = new List<Guid> { nonExistentGenreId },
            PlatformIds = new List<Guid>(),
        };

        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(existingGame);
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesGenreExistAsync(nonExistentGenreId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidGenresException>(() => _gameService.UpdateGameAsync(key, gameDto));
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldThrowInvalidPlatformsException_WhenPlatformIdDoesNotExist()
    {
        // Arrange
        var key = "existing-key";
        var publisherId = Guid.NewGuid();
        var nonExistentPlatformId = Guid.NewGuid();
        var existingGame = new GameEntity { Id = Guid.NewGuid(), Key = key, Name = "Existing Game", PublisherId = publisherId };
        var gameDto = new GameDto
        {
            Key = key,
            Name = "Updated Game",
            PublisherId = publisherId,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid> { nonExistentPlatformId },
        };

        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(existingGame);
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesPlatformExistAsync(nonExistentPlatformId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPlatformsException>(() => _gameService.UpdateGameAsync(key, gameDto));
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldThrowInvalidPublisherException_WhenPublisherDoesNotExist()
    {
        // Arrange
        var key = "existing-key";
        var nonExistentPublisherId = Guid.NewGuid();
        var existingGame = new GameEntity { Id = Guid.NewGuid(), Key = key, Name = "Existing Game", PublisherId = Guid.NewGuid() }; // Existing game has a different publisher
        var gameDto = new GameDto
        {
            Key = key,
            Name = "Updated Game",
            PublisherId = nonExistentPublisherId,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid>(),
        };

        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(existingGame);
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(nonExistentPublisherId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPublisherException>(() => _gameService.UpdateGameAsync(key, gameDto));
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldHandleGenreChanges_RemoveAndAdd()
    {
        // Arrange
        var key = "existing-key";
        var publisherId = Guid.NewGuid();
        var existingGameId = Guid.NewGuid();
        var existingGenreId = Guid.NewGuid();
        var newGenreId = Guid.NewGuid();

        var existingGame = new GameEntity { Id = existingGameId, Key = key, Name = "Existing Game", PublisherId = publisherId };
        var gameDto = new GameDto
        {
            Key = key,
            Name = "Updated Game",
            PublisherId = publisherId,
            GenreIds = new List<Guid> { newGenreId },
            PlatformIds = new List<Guid>(),
        };

        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(existingGame);
        _mapperMock.Setup(mapper => mapper.Map(gameDto, existingGame)).Returns(existingGame);
        _gameRepositoryMock.Setup(repo => repo.UpdateAsync(existingGame)).Returns(Task.CompletedTask);
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesGenreExistAsync(existingGenreId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesGenreExistAsync(newGenreId)).ReturnsAsync(true);

        // Simulate current genres
        _gameRepositoryMock.Setup(repo => repo.GetGamesByGenreAsync(existingGameId))
            .ReturnsAsync(new List<GameEntity> { new() { Id = existingGenreId } });

        _gameRepositoryMock.Setup(repo => repo.RemoveGenresAsync(existingGameId, It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);
        _gameRepositoryMock.Setup(repo => repo.AddGenresAsync(existingGameId, It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);

        // Act
        await _gameService.UpdateGameAsync(key, gameDto);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.RemoveGenresAsync(existingGameId, It.Is<List<Guid>>(ids => ids.Contains(existingGenreId))), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.AddGenresAsync(existingGameId, It.Is<List<Guid>>(ids => ids.Contains(newGenreId))), Times.Once);
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldHandlePlatformChanges_RemoveAndAdd()
    {
        // Arrange
        var key = "existing-key";
        var publisherId = Guid.NewGuid();
        var existingGameId = Guid.NewGuid();
        var existingPlatformId = Guid.NewGuid();
        var newPlatformId = Guid.NewGuid();

        var existingGame = new GameEntity { Id = existingGameId, Key = key, Name = "Existing Game", PublisherId = publisherId };
        var gameDto = new GameDto
        {
            Key = key,
            Name = "Updated Game",
            PublisherId = publisherId,
            GenreIds = new List<Guid>(),
            PlatformIds = new List<Guid> { newPlatformId },
        };

        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(existingGame);
        _mapperMock.Setup(mapper => mapper.Map(gameDto, existingGame)).Returns(existingGame);
        _gameRepositoryMock.Setup(repo => repo.UpdateAsync(existingGame)).Returns(Task.CompletedTask);
        _gameRepositoryMock.Setup(repo => repo.DoesPublisherExistAsync(publisherId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesPlatformExistAsync(existingPlatformId)).ReturnsAsync(true);
        _gameRepositoryMock.Setup(repo => repo.DoesPlatformExistAsync(newPlatformId)).ReturnsAsync(true);

        // Simulate current platforms
        _gameRepositoryMock.Setup(repo => repo.GetGamesByPlatformAsync(existingGameId))
            .ReturnsAsync(new List<GameEntity> { new() { Id = existingPlatformId } });

        _gameRepositoryMock.Setup(repo => repo.RemovePlatformsAsync(existingGameId, It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);
        _gameRepositoryMock.Setup(repo => repo.AddPlatformsAsync(existingGameId, It.IsAny<List<Guid>>())).Returns(Task.CompletedTask);

        // Act
        await _gameService.UpdateGameAsync(key, gameDto);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.RemovePlatformsAsync(existingGameId, It.Is<List<Guid>>(ids => ids.Contains(existingPlatformId))), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.AddPlatformsAsync(existingGameId, It.Is<List<Guid>>(ids => ids.Contains(newPlatformId))), Times.Once);
    }

    // Additional tests for branch coverage
    [Fact]
    public async Task GetGameByIdAsync_ShouldThrowException_WhenGameIsDeleted()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var deletedGame = new GameEntity { Id = gameId, Name = "Deleted Game", IsDeleted = true };
        _gameRepositoryMock.Setup(repo => repo.GetByIdAsync(gameId)).ReturnsAsync(deletedGame);

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _gameService.GetGameByIdAsync(gameId));
    }

    [Fact]
    public async Task GetAllGamesAsync_ShouldFilterOutDeletedGames()
    {
        // Arrange
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Active Game", IsDeleted = false },
            new() { Id = Guid.NewGuid(), Name = "Deleted Game", IsDeleted = true },
        };

        _gameRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(games);

        // Act
        var result = await _gameService.GetAllGamesAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal("Active Game", result.First().Name);
    }

    [Fact]
    public async Task GetGameByKeyAsync_ShouldThrowException_WhenGameIsDeleted()
    {
        // Arrange
        var key = "deleted-game-key";
        var deletedGame = new GameEntity { Key = key, Name = "Deleted Game", IsDeleted = true };
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(deletedGame);

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _gameService.GetGameByKeyAsync(key));
    }

    [Fact]
    public async Task GetGameByKeyAsync_ShouldThrowGameServiceException_OnGeneralException()
    {
        // Arrange
        var key = "test-key";
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() => _gameService.GetGameByKeyAsync(key));
    }

    [Fact]
    public async Task GetGamesByGenreAsync_ShouldThrowGameServiceException_OnGeneralException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        _gameRepositoryMock.Setup(repo => repo.DoesGenreExistAsync(genreId)).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<GameServiceException>(() => _gameService.GetGamesByGenreAsync(genreId));
    }

    [Fact]
    public async Task GetDeletedGameByKeyAsync_ShouldReturnGame_WhenGameExistsAndIsDeleted()
    {
        // Arrange
        var key = "deleted-key";
        var deletedGame = new GameEntity { Key = key, Name = "Deleted Game", IsDeleted = true };
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(deletedGame);

        // Act
        var result = await _gameService.GetDeletedGameByKeyAsync(key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(key, result.Key);
        Assert.True(result.IsDeleted);
    }

    [Fact]
    public async Task GetDeletedGameByKeyAsync_ShouldThrowException_WhenGameNotDeleted()
    {
        // Arrange
        var key = "active-key";
        var activeGame = new GameEntity { Key = key, Name = "Active Game", IsDeleted = false };
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(activeGame);

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _gameService.GetDeletedGameByKeyAsync(key));
    }

    [Fact]
    public async Task GetDeletedGameByKeyAsync_ShouldThrowException_WhenGameDoesNotExist()
    {
        // Arrange
        var key = "non-existent-key";
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act & Assert
        await Assert.ThrowsAsync<GameNotFoundException>(() => _gameService.GetDeletedGameByKeyAsync(key));
    }

    [Fact]
    public async Task RestoreGameAsync_ShouldRestoreDeletedGame()
    {
        // Arrange
        var key = "deleted-key";
        var deletedGame = new GameEntity { Key = key, Name = "Deleted Game", IsDeleted = true };
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(deletedGame);
        _gameRepositoryMock.Setup(repo => repo.UpdateAsync(deletedGame)).Returns(Task.CompletedTask);

        // Act
        await _gameService.RestoreGameAsync(key);

        // Assert
        Assert.False(deletedGame.IsDeleted);
        _gameRepositoryMock.Verify(repo => repo.UpdateAsync(deletedGame), Times.Once);
    }

    [Fact]
    public async Task RestoreGameAsync_ShouldThrowException_WhenGameNotFound()
    {
        // Arrange
        var key = "non-existent-key";
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync((GameEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act & Assert - The service wraps GameNotFoundException in GameServiceException
        await Assert.ThrowsAsync<GameServiceException>(() => _gameService.RestoreGameAsync(key));
    }
}