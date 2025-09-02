using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Authorization;
using AdminPanel.Web.DtoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("platforms")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformService _platformService;

    public PlatformController(IPlatformService platformService)
    {
        _platformService = platformService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformDto>>> GetAllPlatforms()
    {
        try
        {
            var platforms = await _platformService.GetAllPlatformsAsync();
            return Ok(platforms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{Id}")]
    [Authorize]
    public async Task<ActionResult<PlatformDto>> GetPlatformById(Guid id)
    {
        try
        {
            var platform = await _platformService.GetPlatformByIdAsync(id);
            return Ok(platform);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    [RequirePermission(Permissions.AddPlatform)]
    public async Task<IActionResult> Create(PlatformRequest request)
    {
        if (request?.Platform == null)
        {
            return BadRequest(new { error = "Platform object is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Platform.Type))
        {
            return BadRequest(new { error = "The Type field is required." });
        }

        if (request.Platform.Id == Guid.Empty)
        {
            request.Platform.Id = Guid.NewGuid();
        }

        try
        {
            request.Platform.Id = request.Platform.Id == null || request.Platform.Id == Guid.Empty
                ? Guid.NewGuid()
                : request.Platform.Id;

            await _platformService.AddPlatformAsync(request.Platform);
            return Ok("Platform created successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut]
    [RequirePermission(Permissions.UpdatePlatform)]
    public async Task<IActionResult> Update([FromBody] PlatformUpdateRequest request)
    {
        if (request?.Platform == null)
        {
            return BadRequest(new { error = "Platform object is required" });
        }

        if (!request.Platform.Id.HasValue || request.Platform.Id == Guid.Empty)
        {
            return BadRequest(new { error = "The Id field is required and must be a valid Guid." });
        }

        if (string.IsNullOrWhiteSpace(request.Platform.Type))
        {
            return BadRequest(new { error = "The Type field is required." });
        }

        try
        {
            await _platformService.UpdatePlatformAsync(request.Platform.Id.Value, request.Platform); // Ensure Id is non-null
            return Ok("Platform updated successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.DeletePlatform)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _platformService.DeletePlatformAsync(id);
            return Ok("Platform deleted successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("/games/{key}/platforms")]
    [RequirePermission(Permissions.ViewPlatform)]
    public async Task<ActionResult<IEnumerable<PlatformDto>>> GetPlatformsByGameKey(string key)
    {
        try
        {
            var platforms = await _platformService.GetPlatformsByGameKeyAsync(key);
            return platforms == null || !platforms.Any() ? (ActionResult<IEnumerable<PlatformDto>>)NotFound($"No platforms found for game key: {key}") : Ok(platforms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}