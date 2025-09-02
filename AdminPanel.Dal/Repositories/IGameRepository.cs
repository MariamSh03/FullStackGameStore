using AdminPanel.Entity;

namespace AdminPanel.Dal.Repositories;

public interface IGameRepository : IGenericRepository<GameEntity>
{
    Task DeleteAsync(string key);

    Task AddGenresAsync(Guid gameId, List<Guid> genreIds);

    Task AddPlatformsAsync(Guid gameId, List<Guid> platformIds);

    Task<IEnumerable<GameEntity>> GetGamesByGenreAsync(Guid genreId);

    Task<IEnumerable<GameEntity>> GetGamesByPlatformAsync(Guid platformId);

    Task<IEnumerable<GameEntity>> GetGamesByPublisherAsync(string publisherName);

    Task<List<Guid>> GetPublisherIdsByNamesAsync(List<string> publisherNames);

    Task<GameEntity> GetByKeyAsync(string key);

    IQueryable<GameEntity> GetQueryable();

    Task<bool> DoesGenreExistAsync(Guid genreId);

    Task<bool> DoesPlatformExistAsync(Guid platformId);

    Task<bool> DoesPublisherExistAsync(string publisherName);

    Task<bool> DoesPublisherExistAsync(Guid publisherId);

    Task RemoveGenresAsync(Guid gameId, List<Guid> genreIds);

    Task RemovePlatformsAsync(Guid gameId, List<Guid> platformIds);

    IQueryable<GameGenreEntity> GetGameGenreQueryable();

    IQueryable<GamePlatformEntity> GetGamePlatformQueryable();
}
