using AdminPanel.Bll.DTOs;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Interfaces;

public interface IGameLocalizationService
{
    Task<GameResponseDto> GetLocalizedGameAsync(GameEntity game, string language);

    Task<IEnumerable<GameResponseDto>> GetLocalizedGamesAsync(IEnumerable<GameEntity> games, string language);

    string ParseAcceptLanguageHeader(string? acceptLanguageHeader);
}
