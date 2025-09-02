using AdminPanel.Bll.DTOs;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreEntity>> GetAllGenresAsync();

    Task<GenreEntity> GetGenreByIdAsync(Guid id);

    Task<IEnumerable<GenreEntity>> GetGenresByGameKey(string key);

    Task<IEnumerable<GenreEntity>> GetGenresByParentIdAsync(Guid parentId);

    Task AddGenreAsync(GenreDto genre);

    Task AddGenreAsync(string name);

    Task UpdateGenreAsync(Guid id, GenreDto genre);

    Task DeleteGenreAsync(Guid id);
}
