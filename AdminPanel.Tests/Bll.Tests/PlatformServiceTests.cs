using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Services;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using Moq;

namespace AdminPanel.Tests.Bll.Tests;

public class PlatformServiceTests
{
    private readonly Mock<IPlatformRepository> _mockPlatformRepository;
    private readonly PlatformService _platformService;

    public PlatformServiceTests()
    {
        _mockPlatformRepository = new Mock<IPlatformRepository>();
        _platformService = new PlatformService(_mockPlatformRepository.Object);
    }

    [Fact]
    public async Task GetAllPlatformsAsync_ShouldReturnAllPlatforms()
    {
        // Arrange
        var platforms = new List<PlatformEntity>
        {
            new() { Id = Guid.NewGuid(), Type = "PC" },
            new() { Id = Guid.NewGuid(), Type = "Console" },
        };

        _mockPlatformRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(platforms);

        // Act
        var result = await _platformService.GetAllPlatformsAsync();

        // Assert
        Assert.Equal(platforms, result);
    }

    [Fact]
    public async Task GetPlatformByIdAsync_WithValidId_ShouldReturnPlatform()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var platform = new PlatformEntity { Id = platformId, Type = "PC" };

        _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(platformId))
            .ReturnsAsync(platform);

        // Act
        var result = await _platformService.GetPlatformByIdAsync(platformId);

        // Assert
        Assert.Equal(platform, result);
    }

    [Fact]
    public async Task GetPlatformByIdAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var platformId = Guid.NewGuid();

        _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(platformId))
            .ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _platformService.GetPlatformByIdAsync(platformId));
    }

    [Fact]
    public async Task AddPlatformAsync_WithValidDto_ShouldAddPlatform()
    {
        // Arrange
        var platformDto = new PlatformDto { Type = "PC" };

        // Act
        await _platformService.AddPlatformAsync(platformDto);

        // Assert
        _mockPlatformRepository.Verify(
            repo => repo.AddAsync(It.Is<PlatformEntity>(p =>
            p.Type == platformDto.Type &&
            p.Id != Guid.Empty)),
            Times.Once);
    }

    [Fact]
    public async Task AddPlatformAsync_WithNullDto_ShouldThrowInvalidDataException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidDataException>(() => _platformService.AddPlatformAsync(null!));
    }

    [Fact]
    public async Task UpdatePlatformAsync_WithValidIdAndDto_ShouldUpdatePlatform()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var platformDto = new PlatformDto { Type = "Updated PC" };
        var existingPlatform = new PlatformEntity { Id = platformId, Type = "PC" };

        _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(platformId))
            .ReturnsAsync(existingPlatform);

        // Act
        await _platformService.UpdatePlatformAsync(platformId, platformDto);

        // Assert
        _mockPlatformRepository.Verify(
            repo => repo.UpdateAsync(It.Is<PlatformEntity>(p =>

            p.Id == platformId &&
            p.Type == platformDto.Type)),
            Times.Once);
    }

    [Fact]
    public async Task UpdatePlatformAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var platformDto = new PlatformDto { Type = "Updated PC" };

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(platformId))
            .ReturnsAsync((PlatformEntity?)null); // Note the nullable PlatformEntity?
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _platformService.UpdatePlatformAsync(platformId, platformDto));
    }

    [Fact]
    public async Task DeletePlatformAsync_WithValidId_ShouldDeletePlatform()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var platform = new PlatformEntity { Id = platformId, Type = "PC" };

        _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(platformId))
            .ReturnsAsync(platform);

        // Act
        await _platformService.DeletePlatformAsync(platformId);

        // Assert
        _mockPlatformRepository.Verify(repo => repo.DeleteAsync(platform), Times.Once);
    }

    [Fact]
    public async Task DeletePlatformAsync_WithInvalidId_ShouldNotDeletePlatform()
    {
        // Arrange
        var platformId = Guid.NewGuid();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockPlatformRepository.Setup(repo => repo.GetByIdAsync(platformId))
            .ReturnsAsync((PlatformEntity?)null); // Explicitly specify null return
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        await _platformService.DeletePlatformAsync(platformId);

        // Assert
        _mockPlatformRepository.Verify(repo => repo.DeleteAsync(It.IsAny<PlatformEntity>()), Times.Never);
    }

    [Fact]
    public async Task GetPlatformsByGameKeyAsync_WithValidKey_ShouldReturnPlatforms()
    {
        // Arrange
        var gameKey = "game-key";
        var platforms = new List<PlatformEntity>
        {
            new() { Id = Guid.NewGuid(), Type = "PC" },
            new() { Id = Guid.NewGuid(), Type = "Console" },
        };

        _mockPlatformRepository.Setup(repo => repo.GetPlatformsByGameKeyAsync(gameKey))
            .ReturnsAsync(platforms);

        // Act
        var result = await _platformService.GetPlatformsByGameKeyAsync(gameKey);

        // Assert
        Assert.Equal(platforms, result);
    }

    [Fact]
    public async Task GetPlatformsByGameKeyAsync_WithInvalidKey_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var gameKey = "invalid-game-key";

        _mockPlatformRepository.Setup(repo => repo.GetPlatformsByGameKeyAsync(gameKey))
            .ThrowsAsync(new KeyNotFoundException());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _platformService.GetPlatformsByGameKeyAsync(gameKey));
    }

    // Additional tests for better branch coverage
    [Fact]
    public async Task AddPlatformAsync_ShouldThrowException_WhenPlatformIsNull()
    {
        // Arrange
        PlatformDto platformDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<InvalidDataException>(() => _platformService.AddPlatformAsync(platformDto!));
    }

    [Fact]
    public async Task AddPlatformAsync_ShouldThrowException_WhenPlatformTypeIsEmpty()
    {
        // Arrange
        var platformDto = new PlatformDto { Type = string.Empty };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidDataException>(() => _platformService.AddPlatformAsync(platformDto));
    }

    [Fact]
    public async Task GetPlatformByIdAsync_ShouldReturnNull_WhenPlatformNotFound()
    {
        // Arrange
        var platformId = Guid.NewGuid();
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockPlatformRepository.Setup(r => r.GetByIdAsync(platformId)).ReturnsAsync((PlatformEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        var result = await _platformService.GetPlatformByIdAsync(platformId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdatePlatformAsync_ShouldThrowException_WhenPlatformNotFound()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var platformDto = new PlatformDto { Type = "Updated Platform" };

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockPlatformRepository.Setup(r => r.GetByIdAsync(platformId)).ReturnsAsync((PlatformEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _platformService.UpdatePlatformAsync(platformId, platformDto));
    }

    [Fact]
    public async Task DeletePlatformAsync_ShouldNotThrow_WhenPlatformNotFound()
    {
        // Arrange
        var platformId = Guid.NewGuid();

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockPlatformRepository.Setup(r => r.GetByIdAsync(platformId)).ReturnsAsync((PlatformEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act & Assert - Should not throw any exception
        await _platformService.DeletePlatformAsync(platformId);

        // Verify that DeleteAsync was never called since platform was null
        _mockPlatformRepository.Verify(r => r.DeleteAsync(It.IsAny<PlatformEntity>()), Times.Never);
    }
}