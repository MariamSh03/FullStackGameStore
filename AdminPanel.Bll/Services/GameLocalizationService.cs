using System.Globalization;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Dal.Context;
using AdminPanel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdminPanel.Bll.Services;

public class GameLocalizationService : IGameLocalizationService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GameLocalizationService> _logger;

    private static readonly string[] SupportedLanguages = { "en", "ka", "de" };

    public GameLocalizationService(ApplicationDbContext context, ILogger<GameLocalizationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GameResponseDto> GetLocalizedGameAsync(GameEntity game, string language)
    {
        var localizedGames = await GetLocalizedGamesAsync(new[] { game }, language);
        return localizedGames.First();
    }

    public async Task<IEnumerable<GameResponseDto>> GetLocalizedGamesAsync(IEnumerable<GameEntity> games, string language)
    {
        var gamesList = games.ToList();
        var gameIds = gamesList.Select(g => g.Id).ToList();

        // For English, return games directly from Games table
        if (language == "en")
        {
            return gamesList.Select(game => new GameResponseDto
            {
                Id = game.Id,
                Key = game.Key,
                Name = game.Name,
                Description = game.Description,
                Price = game.Price,
                Discount = game.Discount,
                UnitInStock = game.UnitInStock,
            });
        }

        var localizations = await _context.GameLocalizations
            .Where(gl => gameIds.Contains(gl.GameId) && gl.Language == language)
            .ToListAsync();

        var localizationLookup = localizations.ToDictionary(l => l.GameId);

        return gamesList.Select(game =>
        {
            // Try to get localization, fallback to English from Games table
            var hasLocalization = localizationLookup.TryGetValue(game.Id, out var localization);

            return new GameResponseDto
            {
                Id = game.Id,
                Key = game.Key,
                Name = hasLocalization ? localization!.Title : game.Name,
                Description = hasLocalization ? localization!.Description : game.Description,
                Price = game.Price,
                Discount = game.Discount,
                UnitInStock = game.UnitInStock,
            };
        });
    }

    public string ParseAcceptLanguageHeader(string? acceptLanguageHeader)
    {
        if (string.IsNullOrEmpty(acceptLanguageHeader))
        {
            return "en"; // Default fallback
        }

        try
        {
            var languages = acceptLanguageHeader
                .Split(',')
                .Select(ParseLanguageTag)
                .Where(x => x != null)
                .OrderByDescending(x => x.Quality)
                .ThenBy(x => x.Order)
                .ToList();

            // Find first supported language
            foreach (var lang in languages)
            {
                // Try exact match first (e.g., "ka")
                if (SupportedLanguages.Contains(lang.Code))
                {
                    _logger.LogDebug("Matched language {Language} from Accept-Language header", lang.Code);
                    return lang.Code;
                }

                // Try primary language part (e.g., "ka-GE" -> "ka")
                var primaryCode = lang.Code.Split('-')[0];
                if (SupportedLanguages.Contains(primaryCode))
                {
                    _logger.LogDebug("Matched primary language {Language} from Accept-Language header", primaryCode);
                    return primaryCode;
                }
            }

            _logger.LogDebug("No supported language found in Accept-Language header, using English fallback");
            return "en"; // Fallback to English
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error parsing Accept-Language header: {Header}", acceptLanguageHeader);
            return "en"; // Fallback on error
        }
    }

    private static LanguageTag? ParseLanguageTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            return null;
        }

        var parts = tag.Trim().Split(';');
        var code = parts[0].Trim();

        var quality = 1.0;
        if (parts.Length > 1)
        {
            var qPart = parts[1].Trim();
            if (qPart.StartsWith("q=", StringComparison.Ordinal) && double.TryParse(qPart[2..], NumberStyles.Float, CultureInfo.InvariantCulture, out var q))
            {
                quality = q;
            }
        }

        return new LanguageTag
        {
            Code = code,
            Quality = quality,
            Order = 0, // Will be set by caller based on position
        };
    }

    private class LanguageTag
    {
        public string Code { get; set; } = string.Empty;

        public double Quality { get; set; }

        public int Order { get; set; }
    }
}
