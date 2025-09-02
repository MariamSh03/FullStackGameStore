using System.Text.Json;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdminPanel.Bll.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GameService> _logger;

    public GameService(IGameRepository gameRepository, IMapper mapper, ILogger<GameService> logger)
    {
        _gameRepository = gameRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<GameEntity>> GetAllGamesAsync()
    {
        var allGames = await _gameRepository.GetAllAsync();
        return allGames.Where(g => !g.IsDeleted);
    }

    public async Task<GameEntity> GetGameByIdAsync(Guid id)
    {
        var game = await _gameRepository.GetByIdAsync(id)
            ?? throw new GameNotFoundException(id.ToString(), "Game not found.");

        // Hide deleted games from regular users
        return game.IsDeleted ? throw new GameNotFoundException(id.ToString(), "Game not found.") : game;
    }

    public async Task AddGameAsync(GameDto game)
    {
        ValidateGameParameters(game);
        await AddGameInternalAsync(game);
    }

    public async Task UpdateGameAsync(string key, GameDto game)
    {
        try
        {
            // Validate basic game data
            ValidateGameData(game);

            var existingGame = await _gameRepository.GetByKeyAsync(key)
                ?? throw new GameNotFoundException(key, "Unable to update non-existent game.");

            // Initialize collections to prevent null reference exceptions
            game.GenreIds ??= new List<Guid>();
            game.PlatformIds ??= new List<Guid>();

            // Validate genres, platforms, and publisher
            await ValidateGenresAsync(game.GenreIds);
            await ValidatePlatformsAsync(game.PlatformIds);
            await ValidatePublisherAsync(game.PublisherId);

            // Map new data to existing entity and update the game
            _mapper.Map(game, existingGame);
            await _gameRepository.UpdateAsync(existingGame);

            // Handle genre changes
            await HandleGenreChangesAsync(existingGame, game.GenreIds);

            // Handle platform changes
            await HandlePlatformChangesAsync(existingGame, game.PlatformIds);

            _logger.LogInformation("Game {GameName} with key {GameKey} updated successfully", existingGame.Name, existingGame.Key);
        }
        catch (Exception ex) when (ex is GameNotFoundException or InvalidGenresException or InvalidPlatformsException or InvalidPublisherException)
        {
            _logger.LogWarning(ex, "Game update validation failed for key: {GameKey}", key);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update game with key {GameKey}", key);
            throw new GameServiceException($"Failed to update game with key {key}: {ex.Message}", ex);
        }
    }

    public async Task DeleteGameAsync(string key)
    {
        try
        {
            var game = await _gameRepository.GetByKeyAsync(key)
                ?? throw new GameNotFoundException(key, "Unable to delete non-existent game.");

            game.IsDeleted = true;
            await _gameRepository.UpdateAsync(game);
            _logger.LogInformation("Game with key {GameKey} soft deleted successfully", key);
        }
        catch (GameNotFoundException ex)
        {
            _logger.LogWarning(ex, "Attempted to delete a non-existent game with key: {GameKey}", key);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete game with key {GameKey}", key);
            throw new GameServiceException($"Failed to delete game with key {key}: {ex.Message}", ex);
        }
    }

    public async Task DeleteGamebyIdAsync(Guid gameId)
    {
        try
        {
            var game = await _gameRepository.GetByIdAsync(gameId)
                ?? throw new GameNotFoundException("Unable to delete non-existent game.");

            game.IsDeleted = true;

            await _gameRepository.UpdateAsync(game);
            _logger.LogInformation("Game with key {GameKey} soft deleted successfully", gameId);
        }
        catch (GameNotFoundException ex)
        {
            _logger.LogWarning(ex, "Attempted to delete a non-existent game with key: {GameKey}", gameId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete game with key {GameKey}", gameId);
            throw new GameServiceException($"Failed to delete game with key {gameId}: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByGenreAsync(Guid genreId)
    {
        try
        {
            return !await _gameRepository.DoesGenreExistAsync(genreId)
                ? throw new InvalidGenresException(new[] { genreId }, $"Genre with ID '{genreId}' does not exist.")
                : await _gameRepository.GetGamesByGenreAsync(genreId);
        }
        catch (InvalidGenresException ex)
        {
            _logger.LogWarning(ex, "Attempted to get games for a non-existent genre: {GenreId}", genreId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get games by genre {GenreId}", genreId);
            throw new GameServiceException($"Failed to get games by genre {genreId}: {ex.Message}", ex);
        }
    }

    public async Task<GameEntity> GetGameByKeyAsync(string key)
    {
        try
        {
            var game = await _gameRepository.GetByKeyAsync(key);
            if (game == null)
            {
                _logger.LogWarning("Game with key {GameKey} not found", key);
                throw new GameNotFoundException(key, "Game not found.");
            }

            // Hide deleted games from regular users
            if (game.IsDeleted)
            {
                _logger.LogWarning("Attempted to access deleted game with key {GameKey}", key);
                throw new GameNotFoundException(key, "Game not found.");
            }

            return game;
        }
        catch (GameNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get game by key {GameKey}", key);
            throw new GameServiceException($"Failed to get game by key {key}: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByPlatformAsync(Guid platformId)
    {
        try
        {
            return !await _gameRepository.DoesPlatformExistAsync(platformId)
                ? throw new InvalidPlatformsException(new[] { platformId }, $"Platform with ID '{platformId}' does not exist.")
                : await _gameRepository.GetGamesByPlatformAsync(platformId);
        }
        catch (InvalidPlatformsException ex)
        {
            _logger.LogWarning(ex, "Attempted to get games for a non-existent platform: {PlatformId}", platformId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get games by platform {PlatformId}", platformId);
            throw new GameServiceException($"Failed to get games by platform {platformId}: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByPublisherAsync(string publisherName)
    {
        try
        {
            return !await _gameRepository.DoesPublisherExistAsync(publisherName)
                ? throw new InvalidPublisherException(publisherName, $"Publisher '{publisherName}' does not exist.")
                : await _gameRepository.GetGamesByPublisherAsync(publisherName);
        }
        catch (InvalidPublisherException ex)
        {
            _logger.LogWarning(ex, "Attempted to get games for a non-existent publisher: {PublisherName}", publisherName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get games by publisher {PublisherName}", publisherName);
            throw new GameServiceException($"Failed to get games by publisher {publisherName}: {ex.Message}", ex);
        }
    }

    public async Task<Stream> GetGameFileAsync(string key)
    {
        try
        {
            var game = await _gameRepository.GetByKeyAsync(key)
                ?? throw new GameNotFoundException(key, "Game not found.");

            var serializedGame = JsonSerializer.Serialize(game);
            var fileName = $"_{key}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt";
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            await writer.WriteAsync(serializedGame);
            await writer.FlushAsync();
            memoryStream.Position = 0;

            _logger.LogInformation("Game file for key {GameKey} generated as {FileName}", key, fileName);
            return memoryStream;
        }
        catch (GameNotFoundException ex)
        {
            _logger.LogWarning(ex, "Attempted to get file for a non-existent game with key: {GameKey}", key);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate game file for key {GameKey}", key);
            throw new GameServiceException($"Failed to generate game file for key {key}: {ex.Message}", ex);
        }
    }

    public async Task<int> GetTotalGamesCountAsync()
    {
        try
        {
            var games = await _gameRepository.GetAllAsync();
            return games.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get total games count");
            throw new GameServiceException("Failed to get total games count", ex);
        }
    }

    public Task IncrementViewCountAsync(Guid gameId)
    {
        throw new NotImplementedException();
    }

    public Task IncrementViewCountForGamesAsync(List<string> gameIds)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedGamesResultDto> GetFilteredGamesAsync(GameFilterDto filterDto)
    {
        try
        {
            var query = _gameRepository.GetQueryable().Where(g => !g.IsDeleted);

            query = await ApplyFiltersAsync(query, filterDto);
            query = ApplySorting(query, filterDto.Sort ?? string.Empty);

            var totalCount = await query.CountAsync();
            var (currentPage, pageSize, totalPages) = CalculatePagination(filterDto.Page, filterDto.PageCount, totalCount);

            var games = await GetPaginatedGamesAsync(query, currentPage, pageSize);
            var gameResponseDtos = _mapper.Map<IEnumerable<GameEntity>, IEnumerable<GameResponseDto>>(games);

            return new PagedGamesResultDto
            {
                Games = gameResponseDtos,
                TotalPages = totalPages,
                CurrentPage = currentPage,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get filtered games with filter: {Filter}", JsonSerializer.Serialize(filterDto));
            throw new GameServiceException("Failed to retrieve filtered games.", ex);
        }
    }

    // Admin-only methods for managing deleted games
    public async Task<IEnumerable<GameEntity>> GetDeletedGamesAsync()
    {
        var allGames = await _gameRepository.GetAllAsync();
        return allGames.Where(g => g.IsDeleted);
    }

    public async Task<GameEntity> GetDeletedGameByKeyAsync(string key)
    {
        var game = await _gameRepository.GetByKeyAsync(key);
        return game == null || !game.IsDeleted ? throw new GameNotFoundException(key, "Deleted game not found.") : game;
    }

    public async Task RestoreGameAsync(string key)
    {
        try
        {
            var game = await _gameRepository.GetByKeyAsync(key);
            if (game == null || !game.IsDeleted)
            {
                throw new GameNotFoundException(key, "Deleted game not found.");
            }

            game.IsDeleted = false;
            await _gameRepository.UpdateAsync(game);
            _logger.LogInformation("Game with key {GameKey} restored successfully", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to restore game with key {GameKey}", key);
            throw new GameServiceException($"Failed to restore game with key {key}: {ex.Message}", ex);
        }
    }

    public async Task UpdateDeletedGameAsync(string key, GameDto game)
    {
        try
        {
            // Validate basic game data
            ValidateGameData(game);

            var existingGame = await _gameRepository.GetByKeyAsync(key);
            if (existingGame == null || !existingGame.IsDeleted)
            {
                throw new GameNotFoundException(key, "Deleted game not found.");
            }

            // Initialize collections to prevent null reference exceptions
            game.GenreIds ??= new List<Guid>();
            game.PlatformIds ??= new List<Guid>();

            // Validate genres, platforms, and publisher
            await ValidateGenresAsync(game.GenreIds);
            await ValidatePlatformsAsync(game.PlatformIds);
            await ValidatePublisherAsync(game.PublisherId);

            // Map new data to existing entity and update the game
            _mapper.Map(game, existingGame);

            // Ensure the game remains deleted after update
            existingGame.IsDeleted = true;
            await _gameRepository.UpdateAsync(existingGame);

            // Handle genre changes
            await HandleGenreChangesAsync(existingGame, game.GenreIds);

            // Handle platform changes
            await HandlePlatformChangesAsync(existingGame, game.PlatformIds);

            _logger.LogInformation("Deleted game {GameName} with key {GameKey} updated successfully", existingGame.Name, existingGame.Key);
        }
        catch (Exception ex) when (ex is GameNotFoundException or InvalidGenresException or InvalidPlatformsException or InvalidPublisherException)
        {
            _logger.LogWarning(ex, "Deleted game update validation failed for key: {GameKey}", key);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update deleted game with key {GameKey}", key);
            throw new GameServiceException($"Failed to update deleted game with key {key}: {ex.Message}", ex);
        }
    }

    private async Task<IQueryable<GameEntity>> ApplyFiltersAsync(IQueryable<GameEntity> query, GameFilterDto filterDto)
    {
        query = ApplyNameFilter(query, filterDto.Name ?? string.Empty);
        query = ApplyPriceFilter(query, filterDto.MinPrice, filterDto.MaxPrice);
        query = await ApplyGenreFilterAsync(query, filterDto.GenreIds);
        query = await ApplyPlatformFilterAsync(query, filterDto.PlatformIds);
        query = await ApplyPublisherFilterAsync(query, filterDto.PublisherNames);

        ApplyPublishDateFilter(filterDto.PublishDate ?? string.Empty);

        return query;
    }

    private static IQueryable<GameEntity> ApplyNameFilter(IQueryable<GameEntity> query, string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(g => g.Name.ToLower().Contains(name.ToLower()));
        }

        return query;
    }

    private static IQueryable<GameEntity> ApplyPriceFilter(IQueryable<GameEntity> query, double? minPrice, double? maxPrice)
    {
        if (minPrice.HasValue)
        {
            query = query.Where(g => g.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(g => g.Price <= maxPrice.Value);
        }

        return query;
    }

    private async Task<IQueryable<GameEntity>> ApplyGenreFilterAsync(IQueryable<GameEntity> query, List<Guid> genreIds)
    {
        if (genreIds != null && genreIds.Any())
        {
            var gameGenreQuery = _gameRepository.GetGameGenreQueryable();
            var matchingGameIds = await gameGenreQuery
                .Where(gg => genreIds.Contains(gg.GenreId))
                .Select(gg => gg.GameId)
                .Distinct()
                .ToListAsync();

            query = query.Where(g => matchingGameIds.Contains(g.Id));
        }

        return query;
    }

    private async Task<IQueryable<GameEntity>> ApplyPlatformFilterAsync(IQueryable<GameEntity> query, List<Guid> platformIds)
    {
        if (platformIds != null && platformIds.Any())
        {
            var gamePlatformQuery = _gameRepository.GetGamePlatformQueryable();
            var matchingGameIds = await gamePlatformQuery
                .Where(gp => platformIds.Contains(gp.PlatformId))
                .Select(gp => gp.GameId)
                .Distinct()
                .ToListAsync();

            query = query.Where(g => matchingGameIds.Contains(g.Id));
        }

        return query;
    }

    private async Task<IQueryable<GameEntity>> ApplyPublisherFilterAsync(IQueryable<GameEntity> query, List<string> publisherNames)
    {
        if (publisherNames != null && publisherNames.Any())
        {
            var publisherIds = await _gameRepository.GetPublisherIdsByNamesAsync(publisherNames);
            query = query.Where(g => publisherIds.Contains(g.PublisherId));
        }

        return query;
    }

    private void ApplyPublishDateFilter(string publishDate)
    {
        if (!string.IsNullOrWhiteSpace(publishDate))
        {
            _logger.LogWarning("Filtering by PublishDate requires a 'PublishDate' field in GameEntity and implementation of parsing logic for '{PublishDate}'.", publishDate);
        }
    }

    private IQueryable<GameEntity> ApplySorting(IQueryable<GameEntity> query, string sort)
    {
        return string.IsNullOrWhiteSpace(sort) ? query.OrderBy(g => g.Name) : sort switch
        {
            "Price ASC" => query.OrderBy(g => g.Price),
            "Price DESC" => query.OrderByDescending(g => g.Price),
            "Most popular" => query.OrderBy(g => g.Name),
            "Most commented" => query.OrderByDescending(g => g.Name),
            "New" => query.OrderBy(g => g.Name),
            _ => LogUnsupportedSortAndDefault(query, sort),
        };
    }

    private IQueryable<GameEntity> LogUnsupportedSortAndDefault(IQueryable<GameEntity> query, string sort)
    {
        _logger.LogWarning("Unsupported sort option: {SortOption}. Defaulting to sorting by Name.", sort);
        return query.OrderBy(g => g.Name);
    }

    private static (int CurrentPage, int PageSize, int TotalPages) CalculatePagination(int page, int pageCount, int totalCount)
    {
        var pageSize = pageCount <= 0 ? 10 : pageCount;
        var totalPages = pageSize > 0 ? (int)Math.Ceiling((double)totalCount / pageSize) : 0;

        if (totalPages == 0 && totalCount > 0)
        {
            totalPages = 1;
        }

        var currentPage = page < 1 ? 1 : page;
        if (totalPages > 0 && currentPage > totalPages)
        {
            currentPage = totalPages;
        }

        if (totalCount == 0)
        {
            currentPage = 1;
        }

        return (currentPage, pageSize, totalPages);
    }

    private static async Task<List<GameEntity>> GetPaginatedGamesAsync(IQueryable<GameEntity> query, int currentPage, int pageSize)
    {
        return await query
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    private static string GenerateKeyFromName(string name)
    {
        return name.ToLower().Replace(" ", "-");
    }

    private static void SetGameKeyIfNeeded(GameDto game)
    {
        if (string.IsNullOrEmpty(game.Key))
        {
            game.Key = GenerateKeyFromName(game.Name);
        }
    }

    private async Task EnsureGameKeyIsUnique(GameDto game)
    {
        var existingGame = await _gameRepository.GetByKeyAsync(game.Key);
        if (existingGame != null)
        {
            throw new GameAlreadyExistsException(game.Key, "A game with this key already exists.");
        }
    }

    private static void InitializeCollections(GameDto game)
    {
        game.GenreIds ??= new List<Guid>();
        game.PlatformIds ??= new List<Guid>();
    }

    private async Task ValidateGenres(GameDto game)
    {
        var invalidGenres = new List<Guid>();
        foreach (var genreId in game.GenreIds)
        {
            if (!await _gameRepository.DoesGenreExistAsync(genreId))
            {
                invalidGenres.Add(genreId);
            }
        }

        if (invalidGenres.Any())
        {
            throw new InvalidGenresException(invalidGenres, "One or more genre IDs do not exist.");
        }
    }

    private async Task ValidatePlatforms(GameDto game)
    {
        var invalidPlatforms = new List<Guid>();
        foreach (var platformId in game.PlatformIds)
        {
            if (!await _gameRepository.DoesPlatformExistAsync(platformId))
            {
                invalidPlatforms.Add(platformId);
            }
        }

        if (invalidPlatforms.Any())
        {
            throw new InvalidPlatformsException(invalidPlatforms, "One or more platform IDs do not exist.");
        }
    }

    private async Task ValidatePublisher(GameDto game)
    {
        if (!Guid.TryParse(game.PublisherId.ToString(), out _))
        {
            throw new InvalidPublisherException(game.PublisherId.ToString(), "Invalid publisher ID format.");
        }

        if (!await _gameRepository.DoesPublisherExistAsync(game.PublisherId))
        {
            throw new InvalidPublisherException(game.PublisherId.ToString(), "Publisher does not exist.");
        }
    }

    private async Task ValidateGenresAsync(List<Guid> genreIds)
    {
        var invalidGenres = new List<Guid>();
        foreach (var genreId in genreIds)
        {
            if (!await _gameRepository.DoesGenreExistAsync(genreId))
            {
                invalidGenres.Add(genreId);
            }
        }

        if (invalidGenres.Any())
        {
            throw new InvalidGenresException(invalidGenres, "One or more genre IDs do not exist.");
        }
    }

    private async Task ValidatePlatformsAsync(List<Guid> platformIds)
    {
        var invalidPlatforms = new List<Guid>();
        foreach (var platformId in platformIds)
        {
            if (!await _gameRepository.DoesPlatformExistAsync(platformId))
            {
                invalidPlatforms.Add(platformId);
            }
        }

        if (invalidPlatforms.Any())
        {
            throw new InvalidPlatformsException(invalidPlatforms, "One or more platform IDs do not exist.");
        }
    }

    private async Task ValidatePublisherAsync(Guid publisherId)
    {
        if (!await _gameRepository.DoesPublisherExistAsync(publisherId))
        {
            throw new InvalidPublisherException(publisherId.ToString(), "Publisher does not exist.");
        }
    }

    private async Task HandleGenreChangesAsync(GameEntity existingGame, List<Guid> newGenreIds)
    {
        var currentGenres = (await _gameRepository.GetGamesByGenreAsync(existingGame.Id))
            .Select(g => g.Id)
            .ToList();

        var genresToRemove = currentGenres.Except(newGenreIds).ToList();
        var genresToAdd = newGenreIds.Except(currentGenres).ToList();

        if (genresToRemove.Any())
        {
            await _gameRepository.RemoveGenresAsync(existingGame.Id, genresToRemove);
        }

        if (genresToAdd.Any())
        {
            await _gameRepository.AddGenresAsync(existingGame.Id, genresToAdd);
        }
    }

    private async Task HandlePlatformChangesAsync(GameEntity existingGame, List<Guid> newPlatformIds)
    {
        var currentPlatforms = (await _gameRepository.GetGamesByPlatformAsync(existingGame.Id))
            .Select(g => g.Id)
            .ToList();

        var platformsToRemove = currentPlatforms.Except(newPlatformIds).ToList();
        var platformsToAdd = newPlatformIds.Except(currentPlatforms).ToList();

        if (platformsToRemove.Any())
        {
            await _gameRepository.RemovePlatformsAsync(existingGame.Id, platformsToRemove);
        }

        if (platformsToAdd.Any())
        {
            await _gameRepository.AddPlatformsAsync(existingGame.Id, platformsToAdd);
        }
    }

    private static void ValidateGameData(GameDto game)
    {
        if (game == null)
        {
            throw new GameServiceException("Game data cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(game.Name))
        {
            throw new GameServiceException("Game name is required.");
        }

        if (game.Price < 0)
        {
            throw new GameServiceException("Game price cannot be negative.");
        }

        if (game.UnitInStock < 0)
        {
            throw new GameServiceException("Game units in stock cannot be negative.");
        }

        if (game.Discount is < 0 or > 100)
        {
            throw new GameServiceException("Game discount must be between 0 and 100.");
        }
    }

    private static void ValidateGameParameters(GameDto game)
    {
        if (game == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        ValidateGameData(game);
    }

    private async Task AddGameInternalAsync(GameDto game)
    {
        // Generate a key if not provided
        SetGameKeyIfNeeded(game);

        // Ensure the game key is unique
        await EnsureGameKeyIsUnique(game);

        // Initialize collections to avoid null reference issues
        InitializeCollections(game);

        // Validate genre IDs
        await ValidateGenres(game);

        // Validate platform IDs
        await ValidatePlatforms(game);

        // Validate publisher
        await ValidatePublisher(game);

        // Map to entity
        var gameEntity = _mapper.Map<GameEntity>(game);

        // Save the game
        await _gameRepository.AddAsync(gameEntity);

        // Save related genres and platforms
        await _gameRepository.AddGenresAsync(gameEntity.Id, game.GenreIds);
        await _gameRepository.AddPlatformsAsync(gameEntity.Id, game.PlatformIds);

        // Log the creation
        _logger.LogInformation("Game '{GameName}' with key '{GameKey}' added successfully.", game.Name, game.Key);
    }
}