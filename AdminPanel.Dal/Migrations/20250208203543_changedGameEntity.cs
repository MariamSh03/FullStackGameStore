using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class ChangedGameEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Genres",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ParentGenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Genres", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Platforms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Platforms", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PublisherEntity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                HomePage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PublisherEntity", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Games",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Price = table.Column<double>(type: "float", nullable: false),
                UnitInStock = table.Column<int>(type: "int", nullable: false),
                Discount = table.Column<int>(type: "int", nullable: false),
                PublisherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Games", x => x.Id);
                table.ForeignKey(
                    name: "FK_Games_PublisherEntity_PublisherId",
                    column: x => x.PublisherId,
                    principalTable: "PublisherEntity",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "GameGenres",
            columns: table => new
            {
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameGenres", x => new { x.GameId, x.GenreId });
                table.ForeignKey(
                    name: "FK_GameGenres_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GameGenres_Genres_GenreId",
                    column: x => x.GenreId,
                    principalTable: "Genres",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "GamePlatforms",
            columns: table => new
            {
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GamePlatforms", x => new { x.GameId, x.PlatformId });
                table.ForeignKey(
                    name: "FK_GamePlatforms_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GamePlatforms_Platforms_PlatformId",
                    column: x => x.PlatformId,
                    principalTable: "Platforms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("04341f67-6cb0-4e21-ad8f-23ebbe472c38"), "Puzzle & Skill", null },
                { new Guid("08976868-9a24-4151-9ebd-74a831fbc601"), "FPS", new Guid("db16a27d-8a39-485e-8ba6-498a33eaaeb0") },
                { new Guid("0b16f5b2-b5e6-4a30-9422-0845f75670c8"), "TBS", new Guid("f78a674a-04dd-4eee-8e68-8d71d7ae6a3b") },
                { new Guid("0cdea00c-1854-4eb9-b8ae-58ae868a182d"), "Adventure", null },
                { new Guid("0e2468b0-6256-4b2b-889c-96a8b0a3bf86"), "RPG", null },
                { new Guid("18f3a7fd-6153-4307-8767-d726418a05da"), "Off-road", new Guid("d43e7905-4687-4bdd-bacc-63c217c98278") },
                { new Guid("2345ceb7-6054-4f20-bf55-dd4d60671555"), "Rally", new Guid("d43e7905-4687-4bdd-bacc-63c217c98278") },
                { new Guid("67bf4797-685c-4e31-ac8e-1bc9e804aeff"), "TPS", new Guid("db16a27d-8a39-485e-8ba6-498a33eaaeb0") },
                { new Guid("7218732f-48b4-40b8-a6f2-ca38412c23cf"), "Sports", null },
                { new Guid("87a545b1-d122-4f55-9ed2-bf3aa00a6ea0"), "Arcade", new Guid("d43e7905-4687-4bdd-bacc-63c217c98278") },
                { new Guid("99673268-bc84-420b-81e3-1579fe4924b1"), "RTS", new Guid("f78a674a-04dd-4eee-8e68-8d71d7ae6a3b") },
                { new Guid("c83856b5-cc5a-4592-8602-39b852631735"), "Formula", new Guid("d43e7905-4687-4bdd-bacc-63c217c98278") },
                { new Guid("d43e7905-4687-4bdd-bacc-63c217c98278"), "Races", null },
                { new Guid("db16a27d-8a39-485e-8ba6-498a33eaaeb0"), "Action", null },
                { new Guid("f78a674a-04dd-4eee-8e68-8d71d7ae6a3b"), "Strategy", null },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("01ac193e-6bd2-43f0-9087-38c5b9b3251e"), "Mobile" },
                { new Guid("2d252d62-7deb-41e1-a948-46b8a1f058ab"), "Browser" },
                { new Guid("9b00c04c-be18-4259-8d2e-292648901711"), "Console" },
                { new Guid("a360872e-3f94-494e-bdda-826c95c8efbe"), "Desktop" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameGenres_GenreId",
            table: "GameGenres",
            column: "GenreId");

        migrationBuilder.CreateIndex(
            name: "IX_GamePlatforms_PlatformId",
            table: "GamePlatforms",
            column: "PlatformId");

        migrationBuilder.CreateIndex(
            name: "IX_Games_PublisherId",
            table: "Games",
            column: "PublisherId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameGenres");

        migrationBuilder.DropTable(
            name: "GamePlatforms");

        migrationBuilder.DropTable(
            name: "Genres");

        migrationBuilder.DropTable(
            name: "Games");

        migrationBuilder.DropTable(
            name: "Platforms");

        migrationBuilder.DropTable(
            name: "PublisherEntity");
    }
}
