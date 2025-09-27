using AdminPanel.Dal.Context;
using AdminPanel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdminPanel.Dal.Seeders;

public class GameLocalizationSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GameLocalizationSeeder> _logger;

    public GameLocalizationSeeder(ApplicationDbContext context, ILogger<GameLocalizationSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedGameLocalizationsAsync()
    {
        try
        {
            _logger.LogInformation("Starting game and localization seeding...");

            // First, seed English games in the Games table
            await SeedEnglishGamesAsync();

            // Then, seed translations in GameLocalizations table
            await SeedGameTranslationsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while seeding game localizations");
            throw;
        }
    }

    private async Task SeedEnglishGamesAsync()
    {
        // Check if games with our specific names already exist
        var existingGameNames = new[]
        {
            "Call of Duty: Modern Warfare",
            "The Witcher 3: Wild Hunt",
            "Cyberpunk 2077",
            "FIFA 24",
            "Elden Ring",
        };

        var existingGames = await _context.Games
            .Where(g => existingGameNames.Contains(g.Name))
            .Select(g => g.Name)
            .ToListAsync();

        var gamesToAdd = new List<GameEntity>();

        // Sample English games data
        var gameData = new[]
        {
            new { Name = "Call of Duty: Modern Warfare", Key = "cod-modern-warfare", Description = "Experience the ultimate first-person shooter with stunning graphics and intense multiplayer action.", Price = 59.99, UnitInStock = 100, Discount = 0 },
            new { Name = "The Witcher 3: Wild Hunt", Key = "witcher-3-wild-hunt", Description = "Embark on an epic adventure in a vast open world filled with monsters, magic, and meaningful choices.", Price = 39.99, UnitInStock = 75, Discount = 20 },
            new { Name = "Cyberpunk 2077", Key = "cyberpunk-2077", Description = "Dive into the dark future of Night City, where technology and humanity collide in spectacular ways.", Price = 49.99, UnitInStock = 50, Discount = 30 },
            new { Name = "FIFA 24", Key = "fifa-24", Description = "Experience the world's most authentic football simulation with realistic gameplay and updated teams.", Price = 69.99, UnitInStock = 200, Discount = 10 },
            new { Name = "Elden Ring", Key = "elden-ring", Description = "Explore a mystical world crafted by Hidetaka Miyazaki and George R.R. Martin in this epic action RPG.", Price = 59.99, UnitInStock = 80, Discount = 15 },
        };

        // Get first publisher for foreign key (create a default one if needed)
        var publisher = await _context.Publishers.FirstOrDefaultAsync();
        if (publisher == null)
        {
            _logger.LogWarning("No publishers found. Creating default publisher for games.");
            publisher = new PublisherEntity
            {
                Id = Guid.NewGuid(),
                CompanyName = "Default Publisher",
                HomePage = "https://example.com",
                Description = "Default publisher for sample games",
            };
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
        }

        foreach (var game in gameData)
        {
            if (!existingGames.Contains(game.Name))
            {
                gamesToAdd.Add(new GameEntity
                {
                    Id = Guid.NewGuid(),
                    Name = game.Name,
                    Key = game.Key,
                    Description = game.Description,
                    Price = game.Price,
                    UnitInStock = game.UnitInStock,
                    Discount = game.Discount,
                    PublisherId = publisher.Id,
                    IsDeleted = false,
                });
            }
        }

        if (gamesToAdd.Any())
        {
            await _context.Games.AddRangeAsync(gamesToAdd);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Added {gamesToAdd.Count} English games to the Games table.");
        }
        else
        {
            _logger.LogInformation("English games already exist. Skipping Games table seeding.");
        }
    }

    private async Task SeedGameTranslationsAsync()
    {
        // Check if localizations already exist
        if (await _context.GameLocalizations.AnyAsync())
        {
            _logger.LogInformation("Game localizations already exist. Skipping localization seeding.");
            return;
        }

        // Get the games we just created/verified
        var gameNames = new[]
        {
            "Call of Duty: Modern Warfare",
            "The Witcher 3: Wild Hunt",
            "Cyberpunk 2077",
            "FIFA 24",
            "Elden Ring",
        };

        var games = await _context.Games
            .Where(g => gameNames.Contains(g.Name))
            .ToListAsync();

        if (!games.Any())
        {
            _logger.LogWarning("No target games found for localization. Please ensure English games are seeded first.");
            return;
        }

        var localizations = new List<GameLocalizationEntity>();

        // Translation data (ONLY Georgian and German - English is in Games table)
        var translationData = new Dictionary<string, (string Language, string Title, string Description)[]>
        {
            ["Call of Duty: Modern Warfare"] = new[]
            {
                ("ka", "მოწოდება მოვალეობისა: თანამედროვე ომი", "გამოცადეთ უკიდურესი პირველი პირის შუტერი შესანიშნავი გრაფიკითა და ინტენსიური მულტიპლეიერ მოქმედებით."),
                ("de", "Call of Duty: Modern Warfare", "Erleben Sie den ultimativen Ego-Shooter mit atemberaubender Grafik und intensiver Multiplayer-Action."),
            },
            ["The Witcher 3: Wild Hunt"] = new[]
            {
                ("ka", "ჯადოქარი 3: ველური ნადირობა", "დაიწყეთ ეპიკური თავგადასავალი ვრცელ ღია სამყაროში, რომელიც სავსეა მონსტრებით, მაგიითა და მნიშვნელოვანი არჩევნებით."),
                ("de", "The Witcher 3: Wilde Jagd", "Begeben Sie sich auf ein episches Abenteuer in einer riesigen offenen Welt voller Monster, Magie und bedeutsamer Entscheidungen."),
            },
            ["Cyberpunk 2077"] = new[]
            {
                ("ka", "კიბერპანკი 2077", "ჩაღრმავდით ღამის ქალაქის მუქ მომავალში, სადაც ტექნოლოგია და კაცობრიობა შეჯახდება სპექტაკულარული გზებით."),
                ("de", "Cyberpunk 2077", "Tauchen Sie ein in die düstere Zukunft von Night City, wo Technologie und Menschlichkeit auf spektakuläre Weise aufeinanderprallen."),
            },
            ["FIFA 24"] = new[]
            {
                ("ka", "ფიფა 24", "გამოცადეთ მსოფლიოს ყველაზე ავთენტური ფეხბურთის სიმულაცია რეალისტური თამაშობითა და განახლებული გუნდებით."),
                ("de", "FIFA 24", "Erleben Sie die authentischste Fußballsimulation der Welt mit realistischem Gameplay und aktualisierten Teams."),
            },
            ["Elden Ring"] = new[]
            {
                ("ka", "ელდენ რგოლი", "გამოიკვლიეთ მისტიკური სამყარო, რომელიც შექმნილია ჰიდეტაკა მიაზაკისა და ჯორჯ მარტინის მიერ ამ ეპიკურ მოქმედების RPG-ში."),
                ("de", "Elden Ring", "Erkunden Sie eine mystische Welt, die von Hidetaka Miyazaki und George R.R. Martin in diesem epischen Action-RPG geschaffen wurde."),
            },
        };

        // Create localizations for each game
        foreach (var game in games)
        {
            if (translationData.TryGetValue(game.Name, out var translations))
            {
                foreach (var (language, title, description) in translations)
                {
                    localizations.Add(new GameLocalizationEntity
                    {
                        Id = Guid.NewGuid(),
                        GameId = game.Id,
                        Language = language,
                        Title = title,
                        Description = description,
                    });
                }
            }
        }

        // Add all localizations to database
        await _context.GameLocalizations.AddRangeAsync(localizations);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Successfully seeded {localizations.Count} game translations for {games.Count} games (Georgian and German only).");
    }
}
