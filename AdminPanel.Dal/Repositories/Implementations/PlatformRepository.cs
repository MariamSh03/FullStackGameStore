using AdminPanel.Dal.Context;
using AdminPanel.Entity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Dal.Repositories.Implementations;
public class PlatformRepository : GenericRepository<PlatformEntity>, IPlatformRepository
{
    private readonly ApplicationDbContext _context;

    public PlatformRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PlatformEntity>> GetPlatformsByGameKeyAsync(string key)
    {
        var platforms = await (from gp in _context.GamePlatforms
                               join g in _context.Games on gp.GameId equals g.Id
                               join p in _context.Platforms on gp.PlatformId equals p.Id
                               where g.Key == key
                               select p)
                              .ToListAsync();

        return platforms;
    }
}
