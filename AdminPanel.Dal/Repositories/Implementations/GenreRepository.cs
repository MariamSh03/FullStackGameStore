using AdminPanel.Dal.Context;
using AdminPanel.Entity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Dal.Repositories.Implementations;
public class GenreRepository : GenericRepository<GenreEntity>, IGenreRepository
{
    private readonly ApplicationDbContext _context;

    public GenreRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GenreEntity>> GetGenresByGameKey(string key)
    {
        var genres = await (from gg in _context.GameGenres
                            join g in _context.Games on gg.GameId equals g.Id
                            join genre in _context.Genres on gg.GenreId equals genre.Id
                            where g.Key == key
                            select genre)
                      .ToListAsync();

        return genres;
    }
}
