using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Authorization;
using AdminPanel.Web.DtoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

public class GameController : Controller
{
    private readonly IGameService _gameService;
    private readonly IOrderService _orderService;

    public GameController(IGameService gameService, IOrderService orderService)
    {
        _gameService = gameService;
        _orderService = orderService;
    }

    [HttpGet("games")]
    public async Task<IActionResult> GetGames([FromQuery] GameFilterDto filter)
    {
        try
        {
            // Handle platforms parameter
            if (Request.Query.ContainsKey("platforms"))
            {
                filter.PlatformIds = Request.Query["platforms"]
                    .Select(p => Guid.TryParse(p, out var guid) ? guid : Guid.Empty)
                    .Where(p => p != Guid.Empty)
                    .ToList();
            }

            // Handle genres parameter
            if (Request.Query.ContainsKey("genres"))
            {
                filter.GenreIds = Request.Query["genres"]
                    .Select(g => Guid.TryParse(g, out var guid) ? guid : Guid.Empty)
                    .Where(g => g != Guid.Empty)
                    .ToList();
            }

            // Call the service with the updated filter
            var result = await _gameService.GetFilteredGamesAsync(filter);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving games." });
        }
    }

    // US16 - Get all games without filters
    [HttpGet("games/all")]
    public async Task<IActionResult> GetAllGames()
    {
        try
        {
            var games = await _gameService.GetAllGamesAsync();
            var gameResponse = games.Select(g => new GameResponseDto
            {
                Id = g.Id,
                Description = g.Description,
                Key = g.Key,
                Name = g.Name,
                Price = g.Price,
                Discount = g.Discount,
                UnitInStock = g.UnitInStock,
            });
            return Ok(gameResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving all games.", error = ex.Message });
        }
    }

    [HttpGet("games/{key}")]
    public async Task<IActionResult> GetGameByKey(string key)
    {
        var game = await _gameService.GetGameByKeyAsync(key);
        return game == null ? NotFound() : Ok(game);
    }

    [HttpGet("games/find/{id}")]
    public async Task<IActionResult> GetGameById(Guid id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        return game == null ? NotFound() : Ok(game);
    }

    [HttpPost("games")]
    [RequirePermission(Permissions.AddGame)]
    public async Task<IActionResult> Create([FromBody] UIRequestFormat uiRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createGameDto = GameRequestMapper.MapToApiFormat(uiRequest);

            // Process the request
            await _gameService.AddGameAsync(createGameDto);
            return Ok("Game created successfully.");
        }
        catch (GameAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (InvalidGenresException ex)
        {
            return BadRequest(new { message = ex.Message, invalidGenres = ex.InvalidGenreIds });
        }
        catch (InvalidPlatformsException ex)
        {
            return BadRequest(new { message = ex.Message, invalidPlatforms = ex.InvalidPlatformIds });
        }
        catch (InvalidPublisherException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An internal error occurred.", error = ex.Message });
        }
    }

    [HttpPut("games")]
    [RequirePermission(Permissions.UpdateGame)]
    public async Task<IActionResult> UpdateGame(string key, [FromBody] UIRequestFormat uiRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Map the UI request to the API format
            var gameDto = GameRequestMapper.MapToApiFormat(uiRequest);

            // Update the game
            await _gameService.UpdateGameAsync(key, gameDto);

            return Ok("Game updated successfully.");
        }
        catch (GameNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidGenresException ex)
        {
            return BadRequest(new { message = ex.Message, invalidGenres = ex.InvalidGenreIds });
        }
        catch (InvalidPlatformsException ex)
        {
            return BadRequest(new { message = ex.Message, invalidPlatforms = ex.InvalidPlatformIds });
        }
        catch (InvalidPublisherException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("games/{key}")]
    [RequirePermission(Permissions.DeleteGame)]
    public async Task<IActionResult> DeleteGameByKey(string key)
    {
        try
        {
            await _gameService.DeleteGameAsync(key);
            return Ok("Deleted successfully");
        }
        catch (Exception ex)
        {
            // Log ex for internal tracking
            return NotFound($"Game with key {key} not found: {ex.Message}");
        }
    }

    [HttpGet("genres/{id}/games")]
    [RequirePermission(Permissions.ViewGame)]
    public async Task<IActionResult> GetGamesByGenre(Guid genreId)
    {
        try
        {
            var games = await _gameService.GetGamesByGenreAsync(genreId);
            return Ok(games);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Handle invalid genre ID
        }
    }

    [HttpGet("platforms/{id}/games")]
    [RequirePermission(Permissions.ViewGame)]
    public async Task<IActionResult> GetGamesByPlatform(Guid platformId)
    {
        try
        {
            var games = await _gameService.GetGamesByPlatformAsync(platformId);
            return Ok(games);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Handle invalid platform ID
        }
    }

    // Admin-only endpoints for managing deleted games
    [HttpGet("games/deleted")]
    [RequirePermission(Permissions.ViewDeletedGames)]
    public async Task<IActionResult> GetDeletedGames()
    {
        try
        {
            var deletedGames = await _gameService.GetDeletedGamesAsync();
            var gameResponse = deletedGames.Select(g => new GameResponseDto
            {
                Id = g.Id,
                Description = g.Description,
                Key = g.Key,
                Name = g.Name,
                Price = g.Price,
                Discount = g.Discount,
                UnitInStock = g.UnitInStock,
            });
            return Ok(gameResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving deleted games.", error = ex.Message });
        }
    }

    [HttpGet("games/deleted/{key}")]
    [RequirePermission(Permissions.ViewDeletedGames)]
    public async Task<IActionResult> GetDeletedGameByKey(string key)
    {
        try
        {
            var game = await _gameService.GetDeletedGameByKeyAsync(key);
            return Ok(game);
        }
        catch (GameNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("games/deleted/{key}")]
    [RequirePermission(Permissions.EditDeletedGames)]
    public async Task<IActionResult> UpdateDeletedGame(string key, [FromBody] UIRequestFormat uiRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updateGameDto = GameRequestMapper.MapToApiFormat(uiRequest);
            await _gameService.UpdateDeletedGameAsync(key, updateGameDto);
            return Ok("Deleted game updated successfully.");
        }
        catch (GameNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidGenresException ex)
        {
            return BadRequest(new { message = ex.Message, invalidGenres = ex.InvalidGenreIds });
        }
        catch (InvalidPlatformsException ex)
        {
            return BadRequest(new { message = ex.Message, invalidPlatforms = ex.InvalidPlatformIds });
        }
        catch (InvalidPublisherException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("games/deleted/{key}/restore")]
    [RequirePermission(Permissions.EditDeletedGames)]
    public async Task<IActionResult> RestoreGame(string key)
    {
        try
        {
            await _gameService.RestoreGameAsync(key);
            return Ok("Game restored successfully.");
        }
        catch (GameNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("publisher/{companyName}/games")]
    [RequirePermission(Permissions.ViewGame)]
    public async Task<IActionResult> GetGamesByPublisher(string publisherName)
    {
        try
        {
            var games = await _gameService.GetGamesByPublisherAsync(publisherName);
            return Ok(games);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("games/{key}/file")]
    [RequirePermission(Permissions.ViewGame)]
    public async Task<IActionResult> DownloadGameFile(string key)
    {
        try
        {
            var gameFileStream = await _gameService.GetGameFileAsync(key);
            var fileName = $"{key}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt";

            return File(gameFileStream, "text/plain", fileName);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{key}/buy")]
    [Authorize]
    public async Task<IActionResult> AddGameToCart(Guid key)
    {
        try
        {
            await _orderService.AddGameToCartAsync(key);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("games/pagination-options")]
    [AllowAnonymous]
    public IActionResult GetPaginationOptions()
    {
        // Return the predefined pagination options
        var paginationOptions = new List<string>
        {
            "10",
            "20",
            "50",
            "100",
            "all",
        };

        return Ok(paginationOptions);
    }

    [HttpGet("games/sorting-options")]
    [AllowAnonymous]
    public IActionResult GetSortingOptions()
    {
        // Return the predefined sorting options
        var sortingOptions = new List<string>
        {
            "Most popular",
            "Most commented",
            "Price ASC",
            "Price DESC",
            "New",
        };

        return Ok(sortingOptions);
    }

    [HttpGet("games/publish-date-options")]
    [AllowAnonymous] // Public information
    public IActionResult GetPublishDateOptions()
    {
        // Return the predefined publish date options
        var publishDateOptions = new List<string>
        {
            "last week",
            "last month",
            "last year",
            "2 years",
            "3 years",
        };

        return Ok(publishDateOptions);
    }
}
