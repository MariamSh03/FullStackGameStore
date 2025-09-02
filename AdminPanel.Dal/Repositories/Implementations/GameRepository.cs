using System.Diagnostics.CodeAnalysis;
using AdminPanel.Dal.Context;
using AdminPanel.Entity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Dal.Repositories.Implementations;

[ExcludeFromCodeCoverage]
public class GameRepository : GenericRepository<GameEntity>, IGameRepository
{
    private readonly ApplicationDbContext _context;

    public GameRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task AddGenresAsync(Guid gameId, List<Guid> genreIds)
    {
        foreach (var genreId in genreIds)
        {
            _context.GameGenres.Add(new GameGenreEntity
            {
                GameId = gameId,
                GenreId = genreId,
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddPlatformsAsync(Guid gameId, List<Guid> platformIds)
    {
        foreach (var platformId in platformIds)
        {
            _context.GamePlatforms.Add(new GamePlatformEntity
            {
                GameId = gameId,
                PlatformId = platformId,
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByGenreAsync(Guid genreId)
    {
        return await _context.Games
            .Where(game => _context.GameGenres
                .Any(gg => gg.GameId == game.Id && gg.GenreId == genreId))
            .ToListAsync();
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByPlatformAsync(Guid platformId)
    {
        return await _context.Games
            .Where(game => _context.GamePlatforms
                .Any(gg => gg.GameId == game.Id && gg.PlatformId == platformId))
            .ToListAsync();
    }

    public async Task<GameEntity> GetByKeyAsync(string key)
    {
        return await _context.Games.FirstOrDefaultAsync(g => g.Key == key);
    }

    public async Task<bool> DoesGenreExistAsync(Guid genreId)
    {
        return await _context.Genres.AnyAsync(genre => genre.Id == genreId);
    }

    public async Task<bool> DoesPlatformExistAsync(Guid platformId)
    {
        return await _context.Platforms.AnyAsync(platform => platform.Id == platformId);
    }

    public async Task<bool> DoesPublisherExistAsync(string publisherName)
    {
        return await _context.Publishers.AnyAsync(publisher => publisher.CompanyName == publisherName);
    }

    public async Task<bool> DoesPublisherExistAsync(Guid publisherId)
    {
        return await _context.Publishers.AnyAsync(publisher => publisher.Id == publisherId);
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByPublisherAsync(string publisherName)
    {
        var publisher = await _context.Publishers
            .FirstOrDefaultAsync(p => p.CompanyName == publisherName);

        return publisher == null
            ? throw new ArgumentException("Publisher not found.")
            : (IEnumerable<GameEntity>)await _context.Games
            .Where(g => g.PublisherId == publisher.Id)
            .ToListAsync();
    }

    public async Task DeleteAsync(string key)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Key == key) ?? throw new ArgumentException("Game not found.");
        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveGenresAsync(Guid gameId, List<Guid> genreIds)
    {
        var genresToRemove = await _context.GameGenres
            .Where(gg => gg.GameId == gameId && genreIds.Contains(gg.GenreId))
            .ToListAsync();

        if (genresToRemove.Any())
        {
            _context.GameGenres.RemoveRange(genresToRemove);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemovePlatformsAsync(Guid gameId, List<Guid> platformIds)
    {
        var platformsToRemove = await _context.GamePlatforms
            .Where(gp => gp.GameId == gameId && platformIds.Contains(gp.PlatformId))
            .ToListAsync();

        if (platformsToRemove.Any())
        {
            _context.GamePlatforms.RemoveRange(platformsToRemove);
            await _context.SaveChangesAsync();
        }
    }

    public IQueryable<GameEntity> GetQueryable()
    {
        return _context.Games.AsQueryable();
    }

    public async Task<List<Guid>> GetPublisherIdsByNamesAsync(List<string> publisherNames)
    {
        return publisherNames == null || !publisherNames.Any()
            ? new List<Guid>()
            : await _context.Publishers
            .Where(p => publisherNames.Contains(p.CompanyName))
            .Select(p => p.Id)
            .ToListAsync();
    }

    public IQueryable<GameGenreEntity> GetGameGenreQueryable()
    {
        return _context.GameGenres.AsQueryable();
    }

    public IQueryable<GamePlatformEntity> GetGamePlatformQueryable()
    {
        return _context.GamePlatforms.AsQueryable();
    }
}