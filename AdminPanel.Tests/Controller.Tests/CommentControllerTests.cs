using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;
public class CommentControllerTests
{
    private readonly Mock<ICommentService> _mockCommentService;
    private readonly CommentController _controller;

    public CommentControllerTests()
    {
        _mockCommentService = new Mock<ICommentService>();
        _controller = new CommentController(_mockCommentService.Object);
    }

    [Fact]
    public async Task AddComment_ReturnsOkResult_WhenValid()
    {
        var request = new CommentRequestDto
        {
            Comment = new() { Name = "Test", Body = "Test Body" },
            ParentId = null,
        };

        var result = await _controller.AddComment("game-key", request);

        Assert.IsType<OkResult>(result);
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
        _mockCommentService.Verify(
            s =>
            s.AddCommentAsync("game-key", "Test", "Test Body", null), Times.Once);
#pragma warning restore SA1117 // Parameters should be on same line or separate lines
    }

    [Theory]
    [InlineData(null, "Body")]
    [InlineData("Name", null)]
    [InlineData("", "Body")]
    [InlineData("Name", "")]
    public async Task AddComment_ReturnsBadRequest_WhenInvalid(string name, string body)
    {
        var request = new CommentRequestDto
        {
            Comment = new() { Name = name, Body = body },
            ParentId = null,
        };

        var result = await _controller.AddComment("game-key", request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("All comment fields are required", badRequest.Value);
    }

    [Fact]
    public async Task GetComments_ReturnsCommentsList()
    {
        var expected = new List<CommentResponseDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Test", Body = "Body", ChildComments = new List<CommentResponseDto>() },
            };

        _mockCommentService.Setup(s => s.GetCommentsAsync("game-key"))
            .ReturnsAsync(expected);

        var result = await _controller.GetComments("game-key");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, okResult.Value);
    }

    [Fact]
    public async Task DeleteComment_ReturnsNoContent()
    {
        var id = Guid.NewGuid();

        var result = await _controller.DeleteComment(id);

        Assert.IsType<NoContentResult>(result);
        _mockCommentService.Verify(s => s.DeleteCommentAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetBanDurations_ReturnsDurations()
    {
        var durations = new List<string> { "1 hour", "1 day" };
        _mockCommentService.Setup(s => s.GetBanDurationsAsync()).ReturnsAsync(durations);

        var result = await _controller.GetBanDurations();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(durations, okResult.Value);
    }

    [Fact]
    public async Task BanUser_ReturnsOkResult()
    {
        var request = new BanRequestDto { User = "test", Duration = "1 day" };

        var result = await _controller.BanUser(request);

        Assert.IsType<OkResult>(result);
        _mockCommentService.Verify(s => s.BanUserAsync("test", "1 day"), Times.Once);
    }

    // Additional tests for branch coverage
    [Fact]
    public async Task AddComment_ReturnsBadRequest_WhenRequestIsNull()
    {
        var result = await _controller.AddComment("game-key", null);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("All comment fields are required", badRequestResult.Value);
    }

    [Fact]
    public async Task AddComment_ReturnsBadRequest_WhenCommentIsNull()
    {
        var request = new CommentRequestDto
        {
            Comment = null,
            ParentId = null,
        };

        var result = await _controller.AddComment("game-key", request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("All comment fields are required", badRequestResult.Value);
    }

    [Theory]
    [InlineData("   ", "ValidBody")]
    [InlineData("ValidName", "   ")]
    public async Task AddComment_ReturnsBadRequest_WhenFieldsAreWhitespace(string name, string body)
    {
        var request = new CommentRequestDto
        {
            Comment = new() { Name = name, Body = body },
            ParentId = null,
        };

        var result = await _controller.AddComment("game-key", request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("All comment fields are required", badRequestResult.Value);
    }

    [Fact]
    public async Task AddComment_WithParentId_CallsServiceCorrectly()
    {
        var parentId = Guid.NewGuid();
        var request = new CommentRequestDto
        {
            Comment = new() { Name = "Test", Body = "Test Body" },
            ParentId = parentId,
        };

        var result = await _controller.AddComment("game-key", request);

        Assert.IsType<OkResult>(result);
        _mockCommentService.Verify(
            s => s.AddCommentAsync("game-key", "Test", "Test Body", parentId), Times.Once);
    }

    [Fact]
    public async Task GetComments_ReturnsEmptyList_WhenNoComments()
    {
        var emptyComments = new List<CommentResponseDto>();
        _mockCommentService.Setup(s => s.GetCommentsAsync("game-key")).ReturnsAsync(emptyComments);

        var result = await _controller.GetComments("game-key");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedComments = Assert.IsAssignableFrom<IEnumerable<CommentResponseDto>>(okResult.Value);
        Assert.Empty(returnedComments);
    }
}