using System.Linq.Expressions;

namespace AdminPanel.Dal.Repositories;

public interface IGenericRepository<T>
    where T : class
{
    Task<T> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task<T> SingleFind(Expression<Func<T, bool>> predicate);
}
