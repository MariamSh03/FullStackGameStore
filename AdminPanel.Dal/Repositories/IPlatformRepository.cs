using AdminPanel.Entity;

namespace AdminPanel.Dal.Repositories;
public interface IPlatformRepository : IGenericRepository<PlatformEntity>
{
    public Task<IEnumerable<PlatformEntity>> GetPlatformsByGameKeyAsync(string key);
}
