using AdminPanel.Entity;

namespace AdminPanel.Dal.Repositories;
public interface IGenreRepository : IGenericRepository<GenreEntity>
{
    public Task<IEnumerable<GenreEntity>> GetGenresByGameKey(string key);
}
