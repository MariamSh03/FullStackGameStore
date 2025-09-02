using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Authorization;
using AdminPanel.Web.DtoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("genres")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await _genreService.GetAllGenresAsync();
        return Ok(genres);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetGenreById(Guid id)
    {
        var genre = await _genreService.GetGenreByIdAsync(id);
        return genre == null ? NotFound($"Genre with ID {id} not found.") : Ok(genre);
    }

    [HttpPost]
    [RequirePermission(Permissions.AddGenre)]
    public async Task<IActionResult> Create([FromBody] CreateGenreRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _genreService.AddGenreAsync(request.Genre);
            return Ok("Genre created successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("/games/{key}/genres")]
    [RequirePermission(Permissions.ViewGenre)]
    public async Task<IActionResult> GetGenresByGameKey(string key)
    {
        var genres = await _genreService.GetGenresByGameKey(key);
        return genres == null || !genres.Any()
            ? NotFound($"No genres found for game with key {key}.")
            : Ok(genres);
    }

    [HttpGet("{id}/genres")]
    [RequirePermission(Permissions.ViewGenre)]
    public async Task<IActionResult> GetGenresByParentId(Guid id)
    {
        var genres = await _genreService.GetGenresByParentIdAsync(id);
        return genres == null || !genres.Any()
            ? NotFound($"No sub-genres found for genre with ID {id}.")
            : Ok(genres);
    }

    [HttpPut("{id}")]
    [RequirePermission(Permissions.UpdateGenre)]
    public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] GenreDto genreDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _genreService.UpdateGenreAsync(id, genreDto);
            return Ok("Genre updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.DeleteGenre)]
    public async Task<IActionResult> DeleteGenre([FromRoute] Guid id)
    {
        try
        {
            await _genreService.DeleteGenreAsync(id);
            return Ok("Genre deleted successfully.");
        }
        catch (Exception ex)
        {
            return NotFound($"Genre with ID {id} not found: {ex.Message}");
        }
    }
}