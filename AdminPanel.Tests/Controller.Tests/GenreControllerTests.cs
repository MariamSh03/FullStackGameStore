using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Entity;
using AdminPanel.Web.Controllers;
using AdminPanel.Web.DtoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;

public class GenreControllerTests
{
    private readonly Mock<IGenreService> _mockGenreService;
    private readonly GenreController _controller;

    public GenreControllerTests()
    {
        _mockGenreService = new Mock<IGenreService>();
        _controller = new GenreController(_mockGenreService.Object);
    }

    [Fact]
    public async Task GetAllGenres_ReturnsOkResult_WithListOfGenres()
    {
        // Arrange
        var expectedGenres = new List<GenreEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Action" },
            new() { Id = Guid.NewGuid(), Name = "Adventure" },
        };
        _mockGenreService.Setup(s => s.GetAllGenresAsync())
            .ReturnsAsync(expectedGenres);

        // Act
        var result = await _controller.GetAllGenres();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenres = Assert.IsAssignableFrom<IEnumerable<GenreEntity>>(okResult.Value);
        Assert.Equal(expectedGenres.Count, returnedGenres.Count());
    }

    [Fact]
    public async Task Create_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var request = new CreateGenreRequest
        {
            Genre = new GenreDto { Name = "New Genre" },
        };

        _mockGenreService.Setup(s => s.AddGenreAsync(It.IsAny<GenreDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Genre created successfully.", okResult.Value);
    }

    [Fact]
    public async Task Create_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateGenreRequest
        {
            Genre = new GenreDto(), // Empty name
        };

        _controller.ModelState.AddModelError("Genre.Name", "Name is required");

        // Act
        var result = await _controller.Create(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        var request = new CreateGenreRequest
        {
            Genre = new GenreDto { Name = "New Genre" },
        };

        _mockGenreService.Setup(s => s.AddGenreAsync(It.IsAny<GenreDto>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Create(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }

    [Fact]
    public async Task GetGenreById_WhenGenreExists_ReturnsOkResult()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genre = new GenreEntity { Id = genreId, Name = "Action" };
        _mockGenreService.Setup(s => s.GetGenreByIdAsync(genreId))
            .ReturnsAsync(genre);

        // Act
        var result = await _controller.GetGenreById(genreId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGenre = Assert.IsType<GenreEntity>(okResult.Value);
        Assert.Equal(genre.Id, returnedGenre.Id);
        Assert.Equal(genre.Name, returnedGenre.Name);
    }

    [Fact]
    public async Task UpdateGenre_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreDto = new GenreDto { Name = "Updated Genre" };
        _mockGenreService.Setup(s => s.UpdateGenreAsync(genreId, genreDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateGenre(genreId, genreDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Genre updated successfully.", okResult.Value);
    }

    [Fact]
    public async Task UpdateGenre_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreDto = new GenreDto(); // Missing required fields
        _controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await _controller.UpdateGenre(genreId, genreDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateGenre_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreDto = new GenreDto { Name = "Updated Genre" };
        _mockGenreService.Setup(s => s.UpdateGenreAsync(genreId, genreDto))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.UpdateGenre(genreId, genreDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }

    [Fact]
    public async Task DeleteGenre_WhenGenreExists_ReturnsOkResult()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        _mockGenreService.Setup(s => s.DeleteGenreAsync(genreId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteGenre(genreId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Genre deleted successfully.", okResult.Value);
    }

    [Fact]
    public async Task DeleteGenre_WhenGenreDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        _mockGenreService.Setup(s => s.DeleteGenreAsync(genreId))
            .ThrowsAsync(new Exception("Genre not found"));

        // Act
        var result = await _controller.DeleteGenre(genreId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains($"Genre with ID {genreId} not found", notFoundResult.Value.ToString());
    }

    // Additional tests for better branch coverage
    [Fact]
    public async Task GetGenreById_WhenGenreIsNull_ReturnsNotFound()
    {
        // Arrange
        var genreId = Guid.NewGuid();
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockGenreService.Setup(s => s.GetGenreByIdAsync(genreId)).ReturnsAsync((GenreEntity)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        var result = await _controller.GetGenreById(genreId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Genre with ID {genreId} not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task Create_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("Genre.Name", "Name is required");
        var request = new CreateGenreRequest { Genre = new GenreDto { Name = string.Empty } };

        // Act
        var result = await _controller.Create(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public async Task Create_WhenServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var request = new CreateGenreRequest { Genre = new GenreDto { Name = "Test Genre" } };
        _mockGenreService.Setup(s => s.AddGenreAsync(request.Genre))
                        .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Create(request);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error: Database error", statusCodeResult.Value);
    }

    [Fact]
    public async Task GetGenresByGameKey_WhenGenresAreNull_ReturnsNotFound()
    {
        // Arrange
        var gameKey = "test-game";
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockGenreService.Setup(s => s.GetGenresByGameKey(gameKey)).ReturnsAsync((IEnumerable<GenreEntity>)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        var result = await _controller.GetGenresByGameKey(gameKey);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"No genres found for game with key {gameKey}.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetGenresByGameKey_WhenGenresAreEmpty_ReturnsNotFound()
    {
        // Arrange
        var gameKey = "test-game";
        _mockGenreService.Setup(s => s.GetGenresByGameKey(gameKey)).ReturnsAsync(new List<GenreEntity>());

        // Act
        var result = await _controller.GetGenresByGameKey(gameKey);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"No genres found for game with key {gameKey}.", notFoundResult.Value);
    }
}