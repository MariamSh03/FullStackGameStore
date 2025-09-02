using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("games")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost("{key}/comments")]
    [RequirePermission(Permissions.CommentOnGames)]
    public async Task<IActionResult> AddComment(string key, [FromBody] CommentRequestDto request)
    {
        if (request?.Comment == null || string.IsNullOrWhiteSpace(request.Comment.Body) || string.IsNullOrWhiteSpace(request.Comment.Name))
        {
            return BadRequest("All comment fields are required");
        }

        await _commentService.AddCommentAsync(key, request.Comment.Name, request.Comment.Body, request.ParentId);
        return Ok();
    }

    [HttpGet("{key}/comments")] // Anyone who can view games can see comments
    public async Task<IActionResult> GetComments(string key)
    {
        var comments = await _commentService.GetCommentsAsync(key);
        return Ok(comments);
    }

    [HttpDelete("{key}/comments/{id}")]
    [RequirePermission(Permissions.DeleteComments)]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        await _commentService.DeleteCommentAsync(id);
        return NoContent();
    }

    [HttpGet("/comments/ban/durations")]
    [RequirePermission(Permissions.BanUsers)]
    public async Task<IActionResult> GetBanDurations()
    {
        var durations = await _commentService.GetBanDurationsAsync();
        return Ok(durations);
    }

    [HttpPost("/comments/ban")]
    [RequirePermission(Permissions.BanUsers)]
    public async Task<IActionResult> BanUser([FromBody] BanRequestDto request)
    {
        await _commentService.BanUserAsync(request.User, request.Duration);
        return Ok();
    }
}
