using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Entity;
using AdminPanel.Web.Controllers;
using AdminPanel.Web.DtoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;

public class PublisherControllerTests
{
    private readonly Mock<IPublisherService> _publisherServiceMock;
    private readonly PublisherController _controller;

    public PublisherControllerTests()
    {
        _publisherServiceMock = new Mock<IPublisherService>();
        _controller = new PublisherController(_publisherServiceMock.Object);
    }

    [Fact]
    public async Task AddPublisher_ReturnsOk_WhenValidData()
    {
        // Arrange
        var publisherDto = new PublisherDto { CompanyName = "Test Publisher" };
        var createPublisherRequest = new CreatePublisherRequest { Publisher = publisherDto };

        // Act
        var result = await _controller.AddPublisher(createPublisherRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Publisher added successfully.", okResult.Value);
        _publisherServiceMock.Verify(s => s.AddPublisherAsync(publisherDto), Times.Once);
    }

    [Fact]
    public async Task AddPublisher_ReturnsBadRequest_WhenInvalidData()
    {
        // Act
        var result = await _controller.AddPublisher(null);

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid publisher data.", objectResult.Value);
    }

    [Fact]
    public async Task GetAllPublishers_ReturnsOk_WithPublishers()
    {
        // Arrange
        var publishers = new List<PublisherEntity> { new() { Id = Guid.NewGuid(), CompanyName = "Publisher1" } };
        _publisherServiceMock.Setup(s => s.GetAllPublishersAsync()).ReturnsAsync(publishers);

        // Act
        var result = await _controller.GetAllPublishers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(publishers, okResult.Value);
    }

    [Fact]
    public async Task GetPublisherByCompanyName_ReturnsPublisher_WhenExists()
    {
        // Arrange
        var publisher = new PublisherEntity { Id = Guid.NewGuid(), CompanyName = "Test Publisher" };
        _publisherServiceMock.Setup(s => s.GetPublisherByCompanyAsync("Test Publisher")).ReturnsAsync(publisher);

        // Act
        var result = await _controller.GetPublisherByCompanyName("Test Publisher");

        // Assert
        Assert.Equal(publisher, result.Value);
    }

    [Fact]
    public async Task DeletePublisher_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        _publisherServiceMock.Setup(s => s.DeletePublisherAsync(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletePublisher(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Publisher deleted successfully.", okResult.Value);
    }

    [Fact]
    public async Task DeletePublisher_ReturnsNotFound_WhenKeyNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _publisherServiceMock.Setup(s => s.DeletePublisherAsync(id)).ThrowsAsync(new KeyNotFoundException("Publisher not found."));

        // Act
        var result = await _controller.DeletePublisher(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Publisher not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdatePublisher_ReturnsOk_WhenValidData()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisherDto = new PublisherDto { CompanyName = "Updated Publisher" };

        _publisherServiceMock.Setup(s => s.UpdatePublisherAsync(publisherId, publisherDto))
            .Returns(Task.CompletedTask);

        var result = await _controller.UpdatePublisher(publisherId, publisherDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Publisher updated successfully.", okResult.Value);
        _publisherServiceMock.Verify(s => s.UpdatePublisherAsync(publisherId, publisherDto), Times.Once);
    }

    [Fact]
    public async Task UpdatePublisher_ReturnsBadRequest_WhenInvalidData()
    {
        // Act
        var result = await _controller.UpdatePublisher(Guid.Empty, null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid publisher data.", badRequestResult.Value);
    }
}
