using AdminPanel.Entity;
using AdminPanel.Entity.Authorization;
using AdminPanel.Entity.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Dal.Context;

public class ApplicationDbContext : IdentityDbContext<UserEntity>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<GameEntity> Games { get; set; }

    public DbSet<GenreEntity> Genres { get; set; }

    public DbSet<PlatformEntity> Platforms { get; set; }

    public DbSet<GameGenreEntity> GameGenres { get; set; }

    public DbSet<GamePlatformEntity> GamePlatforms { get; set; }

    public DbSet<PublisherEntity> Publishers { get; set; }

    public DbSet<OrderEntity> Orders { get; set; }

    public DbSet<OrderGameEntity> OrderGames { get; set; }

    public DbSet<CommentEntity> Comments { get; set; }

    // Localization Entities
    public DbSet<LocalizationEntity> Localizations { get; set; }

    public DbSet<GameLocalizationEntity> GameLocalizations { get; set; }

    public DbSet<GenreLocalizationEntity> GenreLocalizations { get; set; }

    public DbSet<PublisherLocalizationEntity> PublisherLocalizations { get; set; }

    public DbSet<PlatformLocalizationEntity> PlatformLocalizations { get; set; }

    // Authorization Entities
    public DbSet<PermissionEntity> Permissions { get; set; }

    public DbSet<RolePermissionEntity> RolePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure many-to-many relationships
        builder.Entity<GameGenreEntity>()
            .HasKey(gg => new { gg.GameId, gg.GenreId });

        builder.Entity<OrderGameEntity>()
            .HasKey(og => new { og.OrderId, og.ProductId });

        builder.Entity<GameGenreEntity>()
            .HasOne<GameEntity>()
            .WithMany()
            .HasForeignKey(gg => gg.GameId);

        builder.Entity<GameGenreEntity>()
            .HasOne<GenreEntity>()
            .WithMany()
            .HasForeignKey(gg => gg.GenreId);

        builder.Entity<GamePlatformEntity>()
            .HasKey(gp => new { gp.GameId, gp.PlatformId });

        builder.Entity<GamePlatformEntity>()
            .HasOne<GameEntity>()
            .WithMany()
            .HasForeignKey(gp => gp.GameId);

        builder.Entity<GamePlatformEntity>()
            .HasOne<PlatformEntity>()
            .WithMany()
            .HasForeignKey(gp => gp.PlatformId);

        builder.Entity<OrderGameEntity>()
            .HasOne<GameEntity>()
            .WithMany()
            .HasForeignKey(og => og.ProductId);

        builder.Entity<OrderGameEntity>()
            .HasOne<OrderEntity>()
            .WithMany()
            .HasForeignKey(og => og.OrderId);

        builder.Entity<PublisherEntity>()
        .HasIndex(p => p.CompanyName)
        .IsUnique();

        builder.Entity<CommentEntity>()
        .HasOne<CommentEntity>()
        .WithMany()
        .HasForeignKey(c => c.ParentCommentId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CommentEntity>()
        .HasOne<GameEntity>()
        .WithMany()
        .HasForeignKey(c => c.GameId)
        .OnDelete(DeleteBehavior.Cascade);

        // Configure localization entities relationships
        builder.Entity<GameLocalizationEntity>()
            .HasOne(gl => gl.Game)
            .WithMany()
            .HasForeignKey(gl => gl.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<GenreLocalizationEntity>()
            .HasOne(gl => gl.Genre)
            .WithMany()
            .HasForeignKey(gl => gl.GenreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PublisherLocalizationEntity>()
            .HasOne(pl => pl.Publisher)
            .WithMany()
            .HasForeignKey(pl => pl.PublisherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PlatformLocalizationEntity>()
            .HasOne(pl => pl.Platform)
            .WithMany()
            .HasForeignKey(pl => pl.PlatformId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure unique constraints for localization entities
        builder.Entity<GameLocalizationEntity>()
            .HasIndex(gl => new { gl.GameId, gl.LanguageCode })
            .IsUnique();

        builder.Entity<GenreLocalizationEntity>()
            .HasIndex(gl => new { gl.GenreId, gl.LanguageCode })
            .IsUnique();

        builder.Entity<PublisherLocalizationEntity>()
            .HasIndex(pl => new { pl.PublisherId, pl.LanguageCode })
            .IsUnique();

        builder.Entity<PlatformLocalizationEntity>()
            .HasIndex(pl => new { pl.PlatformId, pl.LanguageCode })
            .IsUnique();

        builder.Entity<LocalizationEntity>()
            .HasIndex(l => new { l.EntityId, l.EntityType, l.FieldName, l.LanguageCode })
            .IsUnique();

        // Seed predefined genres and subgenres
        var strategyId = Guid.NewGuid();
        var rtsId = Guid.NewGuid();
        var tbsId = Guid.NewGuid();
        var rpgId = Guid.NewGuid();
        var sportsId = Guid.NewGuid();
        var racesId = Guid.NewGuid();
        var rallyId = Guid.NewGuid();
        var arcadeId = Guid.NewGuid();
        var formulaId = Guid.NewGuid();
        var offRoadId = Guid.NewGuid();
        var actionId = Guid.NewGuid();
        var fpsId = Guid.NewGuid();
        var tpsId = Guid.NewGuid();
        var adventureId = Guid.NewGuid();
        var puzzleSkillId = Guid.NewGuid();

        builder.Entity<GenreEntity>().HasData(
            new GenreEntity { Id = strategyId, Name = "Strategy", ParentGenreId = null },
            new GenreEntity { Id = rpgId, Name = "RPG", ParentGenreId = null },
            new GenreEntity { Id = sportsId, Name = "Sports", ParentGenreId = null },
            new GenreEntity { Id = racesId, Name = "Races", ParentGenreId = null },
            new GenreEntity { Id = actionId, Name = "Action", ParentGenreId = null },
            new GenreEntity { Id = adventureId, Name = "Adventure", ParentGenreId = null },
            new GenreEntity { Id = puzzleSkillId, Name = "Puzzle & Skill", ParentGenreId = null },
            new GenreEntity { Id = rtsId, Name = "RTS", ParentGenreId = strategyId },
            new GenreEntity { Id = tbsId, Name = "TBS", ParentGenreId = strategyId },
            new GenreEntity { Id = rallyId, Name = "Rally", ParentGenreId = racesId },
            new GenreEntity { Id = arcadeId, Name = "Arcade", ParentGenreId = racesId },
            new GenreEntity { Id = formulaId, Name = "Formula", ParentGenreId = racesId },
            new GenreEntity { Id = offRoadId, Name = "Off-road", ParentGenreId = racesId },
            new GenreEntity { Id = fpsId, Name = "FPS", ParentGenreId = actionId },
            new GenreEntity { Id = tpsId, Name = "TPS", ParentGenreId = actionId });

        // Seed predefined platforms
        var mobileId = Guid.NewGuid();
        var desktopId = Guid.NewGuid();
        var consoleId = Guid.NewGuid();
        var browserId = Guid.NewGuid();

        builder.Entity<PlatformEntity>().HasData(
            new PlatformEntity { Id = mobileId, Type = "Mobile" },
            new PlatformEntity { Id = desktopId, Type = "Desktop" },
            new PlatformEntity { Id = consoleId, Type = "Console" },
            new PlatformEntity { Id = browserId, Type = "Browser" });

        builder.Entity<RolePermissionEntity>()
            .HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.Entity<RolePermissionEntity>()
            .HasOne<IdentityRole>()
            .WithMany()
            .HasForeignKey(rp => rp.RoleId);

        builder.Entity<RolePermissionEntity>()
            .HasOne<PermissionEntity>()
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId);

        builder.Entity<UserEntity>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
    }
}