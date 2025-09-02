using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Entity;
using AdminPanel.Web.Controllers;
using AdminPanel.Web.DtoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;
public class PlatformControllerTests
{
    private readonly Mock<IPlatformService> _mockPlatformService;
    private readonly PlatformController _controller;

    public PlatformControllerTests()
    {
        _mockPlatformService = new Mock<IPlatformService>();
        _controller = new PlatformController(_mockPlatformService.Object);
    }

    [Fact]
    public async Task GetAllPlatforms_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        _mockPlatformService.Setup(s => s.GetAllPlatformsAsync())
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetAllPlatforms();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }

    [Fact]
    public async Task GetPlatformById_ReturnsOkWithPlatform()
    {
        // Arrange
        var id = Guid.NewGuid();
        var platformEntity = new PlatformEntity { Id = id, Type = "Console" };
        _mockPlatformService.Setup(s => s.GetPlatformByIdAsync(id))
            .ReturnsAsync(platformEntity);

        // Act
        var result = await _controller.GetPlatformById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPlatform = Assert.IsType<PlatformEntity>(okResult.Value);
        Assert.Equal(id, returnedPlatform.Id);
    }

    [Fact]
    public async Task GetPlatformById_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockPlatformService.Setup(s => s.GetPlatformByIdAsync(id))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetPlatformById(id);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }

    [Fact]
    public async Task Create_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var platform = new PlatformDto { Type = "Mobile" };
        var request = new PlatformRequest { Platform = platform };

        _mockPlatformService.Setup(s => s.AddPlatformAsync(It.IsAny<PlatformDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Platform created successfully!", okResult.Value);
    }

    [Fact]
    public async Task Create_WithNullPlatform_ReturnsBadRequest()
    {
        // Arrange
        PlatformRequest request = null;

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var result = await _controller.Create(request);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Platform object is required", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task Create_WithEmptyType_ReturnsBadRequest()
    {
        // Arrange
        var platform = new PlatformDto { Type = string.Empty };
        var request = new PlatformRequest { Platform = platform };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("The Type field is required", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task Create_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        var platform = new PlatformDto { Type = "Mobile" };
        var request = new PlatformRequest { Platform = platform };

        _mockPlatformService.Setup(s => s.AddPlatformAsync(It.IsAny<PlatformDto>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Create(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }

    [Fact]
    public async Task Update_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var platform = new PlatformDto { Id = id, Type = "Console" };
        var request = new PlatformUpdateRequest { Platform = platform };

        _mockPlatformService.Setup(s => s.UpdatePlatformAsync(id, platform))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Platform updated successfully.", okResult.Value);
    }

    [Fact]
    public async Task Update_WithNullPlatform_ReturnsBadRequest()
    {
        // Arrange
        PlatformUpdateRequest request = null;

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var result = await _controller.Update(request);
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Platform object is required", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task Update_WithEmptyId_ReturnsBadRequest()
    {
        // Arrange
        var platform = new PlatformDto { Type = "Console" };
        var request = new PlatformUpdateRequest { Platform = platform };

        // Act
        var result = await _controller.Update(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("The Id field is required", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task Update_WithEmptyType_ReturnsBadRequest()
    {
        // Arrange
        var platform = new PlatformDto { Id = Guid.NewGuid(), Type = string.Empty };
        var request = new PlatformUpdateRequest { Platform = platform };

        // Act
        var result = await _controller.Update(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("The Type field is required", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task Update_WhenPlatformNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var platform = new PlatformDto { Id = id, Type = "Console" };
        var request = new PlatformUpdateRequest { Platform = platform };

        _mockPlatformService.Setup(s => s.UpdatePlatformAsync(id, platform))
            .ThrowsAsync(new KeyNotFoundException("Platform not found"));

        // Act
        var result = await _controller.Update(request);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("Platform not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task Delete_WhenPlatformExists_ReturnsOkResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockPlatformService.Setup(s => s.DeletePlatformAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Platform deleted successfully.", okResult.Value);
    }

    [Fact]
    public async Task Delete_WhenPlatformNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockPlatformService.Setup(s => s.DeletePlatformAsync(id))
            .ThrowsAsync(new KeyNotFoundException("Platform not found"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("Platform not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task GetPlatformsByGameKey_ReturnsOkWithPlatforms()
    {
        // Arrange
        var gameKey = "test-game";
        var platforms = new List<PlatformEntity>
    {
        new() { Id = Guid.NewGuid(), Type = "Console" },
        new() { Id = Guid.NewGuid(), Type = "Mobile" },
    };
        _mockPlatformService.Setup(s => s.GetPlatformsByGameKeyAsync(gameKey))
            .ReturnsAsync(platforms);

        // Act
        var result = await _controller.GetPlatformsByGameKey(gameKey);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPlatforms = Assert.IsAssignableFrom<IEnumerable<PlatformEntity>>(okResult.Value);  // Changed assertion
        Assert.Equal(2, returnedPlatforms.Count());
    }

    [Fact]
    public async Task GetPlatformsByGameKey_WhenNoPlatformsFound_ReturnsNotFound()
    {
        // Arrange
        var gameKey = "test-game";
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockPlatformService.Setup(s => s.GetPlatformsByGameKeyAsync(gameKey))
            .ReturnsAsync((IEnumerable<PlatformEntity>)null);  // Changed type
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        var result = await _controller.GetPlatformsByGameKey(gameKey);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetPlatformsByGameKey_WhenEmptyCollection_ReturnsNotFound()
    {
        // Arrange
        var gameKey = "test-game";
        _mockPlatformService.Setup(s => s.GetPlatformsByGameKeyAsync(gameKey))
            .ReturnsAsync(new List<PlatformEntity>());  // Changed type

        // Act
        var result = await _controller.GetPlatformsByGameKey(gameKey);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetPlatformsByGameKey_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        var gameKey = "test-game";
        _mockPlatformService.Setup(s => s.GetPlatformsByGameKeyAsync(gameKey))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetPlatformsByGameKey(gameKey);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }
}