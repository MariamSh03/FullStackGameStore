using AdminPanel.Bll.DTOs;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameEntity>> GetAllGamesAsync();

    Task<PagedGamesResultDto> GetFilteredGamesAsync(GameFilterDto filterDto);

    Task<GameEntity> GetGameByIdAsync(Guid id);

    Task AddGameAsync(GameDto game);

    Task UpdateGameAsync(string key, GameDto game);

    Task DeleteGameAsync(string key);

    Task<Stream> GetGameFileAsync(string key);

    Task<IEnumerable<GameEntity>> GetGamesByGenreAsync(Guid genreId);

    Task<GameEntity> GetGameByKeyAsync(string key);

    Task<IEnumerable<GameEntity>> GetGamesByPlatformAsync(Guid platformId);

    Task<IEnumerable<GameEntity>> GetGamesByPublisherAsync(string publisherName);

    Task<int> GetTotalGamesCountAsync();

    // Methods for tracking game views (for NFR1)
    Task IncrementViewCountAsync(Guid gameId);

    Task IncrementViewCountForGamesAsync(List<string> gameIds);

    // Admin-only methods for managing deleted games
    Task<IEnumerable<GameEntity>> GetDeletedGamesAsync();

    Task<GameEntity> GetDeletedGameByKeyAsync(string key);

    Task RestoreGameAsync(string key);

    Task UpdateDeletedGameAsync(string key, GameDto game);
}