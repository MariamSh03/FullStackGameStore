using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Services;

public class PlatformService : IPlatformService
{
    private readonly IPlatformRepository _platformRepository;

    public PlatformService(IPlatformRepository platformRepository)
    {
        _platformRepository = platformRepository;
    }

    public async Task<IEnumerable<PlatformEntity>> GetAllPlatformsAsync()
    {
        return await _platformRepository.GetAllAsync();
    }

    public async Task<PlatformEntity> GetPlatformByIdAsync(Guid id)
    {
        return await _platformRepository.GetByIdAsync(id);
    }

    public async Task DeletePlatformAsync(Guid id)
    {
        var platform = await _platformRepository.GetByIdAsync(id);
        if (platform != null)
        {
            await _platformRepository.DeleteAsync(platform);
        }
    }

    public async Task AddPlatformAsync(PlatformDto platform)
    {
        if (platform == null)
        {
            throw new InvalidDataException("platform is null!");
        }

        if (string.IsNullOrEmpty(platform.Type))
        {
            throw new InvalidDataException("Platform name is invalid!");
        }

        var platformEntity = new PlatformEntity()
        {
            Id = Guid.NewGuid(),
            Type = platform.Type,
        };
        await _platformRepository.AddAsync(platformEntity);
    }

    public async Task UpdatePlatformAsync(Guid id, PlatformDto platform)
    {
        var platformEntity = await _platformRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Platform not found.");

        // Update the platform properties
        platformEntity.Type = platform.Type;  // Assuming you have a Type property

        // Save changes to the database
        await _platformRepository.UpdateAsync(platformEntity);
    }

    public async Task<IEnumerable<PlatformEntity>> GetPlatformsByGameKeyAsync(string key)
    {
        return await _platformRepository.GetPlatformsByGameKeyAsync(key);
    }
}
