using AdminPanel.Bll.DTOs;

namespace AdminPanel.Web.DtoMapper;

public class GameRequestMapper
{
    public static GameDto MapToApiFormat(UIRequestFormat uiRequest)
    {
        return uiRequest == null || uiRequest.Game == null
            ? throw new ArgumentNullException(nameof(uiRequest), "UI request data cannot be null.")
            : new GameDto
            {
                Name = uiRequest.Game.Name,
                Key = uiRequest.Game.Key,
                Description = uiRequest.Game.Description,
                Price = (double)uiRequest.Game.Price, // Ensure this is included in the UI request
                UnitInStock = uiRequest.Game.UnitInStock, // Ensure this is included in the UI request
                Discount = (int)uiRequest.Game.Discount, // Ensure this is included in the UI request
                GenreIds = uiRequest.Genres
                 .Select(g => Guid.TryParse(g, out var guid) ? guid : Guid.Empty)
                     .ToList(),
                PlatformIds = uiRequest.Platforms
                     .Select(p => Guid.TryParse(p, out var guid) ? guid : Guid.Empty)
                         .ToList(),
                PublisherId = Guid.TryParse(uiRequest.Publisher, out var publisherId) ? publisherId : Guid.Empty,
            };
    }
}