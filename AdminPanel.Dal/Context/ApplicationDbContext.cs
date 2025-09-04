using AdminPanel.Entity;
using AdminPanel.Entity.Authorization;
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
        builder.Entity<PlatformEntity>().HasData(
            new PlatformEntity { Id = Guid.NewGuid(), Type = "Mobile" },
            new PlatformEntity { Id = Guid.NewGuid(), Type = "Desktop" },
            new PlatformEntity { Id = Guid.NewGuid(), Type = "Console" },
            new PlatformEntity { Id = Guid.NewGuid(), Type = "Browser" });

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