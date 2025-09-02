using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class AddLocalizationSupport : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("1f943774-8d9c-4632-9e5e-116f319fcfc1"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("22d53603-564a-4ef6-9918-47b2a74b613c"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("268e0e46-4505-4cd3-b74a-cf484d98d98c"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("4d5d3b49-5104-440f-90ac-225283dacd5b"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("54d32d47-d6be-48cd-9fed-66a63e71611f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("66e4debd-0d33-40d4-9949-b30d07c9694f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("8281a2d3-6dae-47b8-8bc7-21332ebb2a9b"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("8a2023bb-df2b-4159-84e3-5705c30ff396"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("936ffefb-a006-4191-8100-20dded5fc9c7"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a395d5d8-c6e5-4344-9be3-5d984e0198b4"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("aef73d6b-4544-4b2c-90d2-46acd83ff47a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b59956e8-8ff3-465c-988d-673937f3c0b4"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f3e266a3-394b-4737-a14e-e06a51ff9dcd"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f3ed9c55-5a2d-4c3b-8f19-b5ea04e45764"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ff9b9e82-b1b6-4a2e-8467-14a23a55a3cd"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("3c2b1528-fa27-450b-b789-4573ed2b3c7c"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("53c519a3-fb44-411f-b180-2e580dd5dd23"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("641b74bc-55c0-4ade-9e5f-fea6aefffd61"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("e0df8119-39d1-4bce-92bf-1f9a15207fa0"));

        migrationBuilder.CreateTable(
            name: "GameLocalizations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameLocalizations", x => x.Id);
                table.ForeignKey(
                    name: "FK_GameLocalizations_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "GenreLocalizations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GenreLocalizations", x => x.Id);
                table.ForeignKey(
                    name: "FK_GenreLocalizations_Genres_GenreId",
                    column: x => x.GenreId,
                    principalTable: "Genres",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Localizations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Localizations", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PlatformLocalizations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PlatformLocalizations", x => x.Id);
                table.ForeignKey(
                    name: "FK_PlatformLocalizations_Platforms_PlatformId",
                    column: x => x.PlatformId,
                    principalTable: "Platforms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PublisherLocalizations",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PublisherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LanguageCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                CompanyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                HomePage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PublisherLocalizations", x => x.Id);
                table.ForeignKey(
                    name: "FK_PublisherLocalizations_Publishers_PublisherId",
                    column: x => x.PublisherId,
                    principalTable: "Publishers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("05642ebf-05c7-48f6-82cf-29fdbc209217"), "Puzzle & Skill", null },
                { new Guid("0e336c0d-2d4d-490b-9851-74bb6edfb40b"), "Strategy", null },
                { new Guid("20be5e86-288d-4ecd-9c0b-e1894beb1e99"), "Races", null },
                { new Guid("5b78b85b-9797-4e9e-8124-9b21816ade77"), "RPG", null },
                { new Guid("5c2e2c42-f1ff-4dc8-be6b-ebc74464f950"), "TPS", new Guid("edabc16d-c23c-4f37-800c-95f9c1af8627") },
                { new Guid("644a2bd1-7383-4e72-8352-cd0596508fa2"), "Sports", null },
                { new Guid("67d66e11-90b5-420e-8cda-12ca5eb67ad2"), "Arcade", new Guid("20be5e86-288d-4ecd-9c0b-e1894beb1e99") },
                { new Guid("79619735-2f47-4499-883b-5b5eb5ba79e0"), "Adventure", null },
                { new Guid("a6b3bde3-bbb6-4dd5-b477-05dcd225f42e"), "Off-road", new Guid("20be5e86-288d-4ecd-9c0b-e1894beb1e99") },
                { new Guid("a95307f8-e569-4958-8780-a9ae3b72808d"), "FPS", new Guid("edabc16d-c23c-4f37-800c-95f9c1af8627") },
                { new Guid("c5d0caf7-aa67-45c2-b720-47ad5a0152fa"), "RTS", new Guid("0e336c0d-2d4d-490b-9851-74bb6edfb40b") },
                { new Guid("d09be634-ea0c-4d3c-87d6-387a7d1bc89f"), "Rally", new Guid("20be5e86-288d-4ecd-9c0b-e1894beb1e99") },
                { new Guid("daea1f5e-8e4a-4d4e-91a9-280aa4ef6bad"), "Formula", new Guid("20be5e86-288d-4ecd-9c0b-e1894beb1e99") },
                { new Guid("edabc16d-c23c-4f37-800c-95f9c1af8627"), "Action", null },
                { new Guid("f3cf4384-84f7-491e-82be-6207196434b9"), "TBS", new Guid("0e336c0d-2d4d-490b-9851-74bb6edfb40b") },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("3db9f881-1791-4241-9c30-eb7e41c3169e"), "Desktop" },
                { new Guid("614f8658-40dc-45ef-af35-a3ae9d6b59ec"), "Console" },
                { new Guid("939e08d2-16ff-432c-899f-7382241034e2"), "Mobile" },
                { new Guid("f056254f-da72-4e4b-8d09-acb9ef5cd430"), "Browser" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameLocalizations_GameId_LanguageCode",
            table: "GameLocalizations",
            columns: new[] { "GameId", "LanguageCode" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_GenreLocalizations_GenreId_LanguageCode",
            table: "GenreLocalizations",
            columns: new[] { "GenreId", "LanguageCode" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Localizations_EntityId_EntityType_FieldName_LanguageCode",
            table: "Localizations",
            columns: new[] { "EntityId", "EntityType", "FieldName", "LanguageCode" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PlatformLocalizations_PlatformId_LanguageCode",
            table: "PlatformLocalizations",
            columns: new[] { "PlatformId", "LanguageCode" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PublisherLocalizations_PublisherId_LanguageCode",
            table: "PublisherLocalizations",
            columns: new[] { "PublisherId", "LanguageCode" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameLocalizations");

        migrationBuilder.DropTable(
            name: "GenreLocalizations");

        migrationBuilder.DropTable(
            name: "Localizations");

        migrationBuilder.DropTable(
            name: "PlatformLocalizations");

        migrationBuilder.DropTable(
            name: "PublisherLocalizations");

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("05642ebf-05c7-48f6-82cf-29fdbc209217"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("0e336c0d-2d4d-490b-9851-74bb6edfb40b"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("20be5e86-288d-4ecd-9c0b-e1894beb1e99"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5b78b85b-9797-4e9e-8124-9b21816ade77"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5c2e2c42-f1ff-4dc8-be6b-ebc74464f950"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("644a2bd1-7383-4e72-8352-cd0596508fa2"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("67d66e11-90b5-420e-8cda-12ca5eb67ad2"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("79619735-2f47-4499-883b-5b5eb5ba79e0"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a6b3bde3-bbb6-4dd5-b477-05dcd225f42e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a95307f8-e569-4958-8780-a9ae3b72808d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("c5d0caf7-aa67-45c2-b720-47ad5a0152fa"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d09be634-ea0c-4d3c-87d6-387a7d1bc89f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("daea1f5e-8e4a-4d4e-91a9-280aa4ef6bad"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("edabc16d-c23c-4f37-800c-95f9c1af8627"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f3cf4384-84f7-491e-82be-6207196434b9"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("3db9f881-1791-4241-9c30-eb7e41c3169e"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("614f8658-40dc-45ef-af35-a3ae9d6b59ec"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("939e08d2-16ff-432c-899f-7382241034e2"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("f056254f-da72-4e4b-8d09-acb9ef5cd430"));

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("1f943774-8d9c-4632-9e5e-116f319fcfc1"), "TPS", new Guid("54d32d47-d6be-48cd-9fed-66a63e71611f") },
                { new Guid("22d53603-564a-4ef6-9918-47b2a74b613c"), "RTS", new Guid("f3ed9c55-5a2d-4c3b-8f19-b5ea04e45764") },
                { new Guid("268e0e46-4505-4cd3-b74a-cf484d98d98c"), "Races", null },
                { new Guid("4d5d3b49-5104-440f-90ac-225283dacd5b"), "Adventure", null },
                { new Guid("54d32d47-d6be-48cd-9fed-66a63e71611f"), "Action", null },
                { new Guid("66e4debd-0d33-40d4-9949-b30d07c9694f"), "Rally", new Guid("268e0e46-4505-4cd3-b74a-cf484d98d98c") },
                { new Guid("8281a2d3-6dae-47b8-8bc7-21332ebb2a9b"), "Puzzle & Skill", null },
                { new Guid("8a2023bb-df2b-4159-84e3-5705c30ff396"), "RPG", null },
                { new Guid("936ffefb-a006-4191-8100-20dded5fc9c7"), "FPS", new Guid("54d32d47-d6be-48cd-9fed-66a63e71611f") },
                { new Guid("a395d5d8-c6e5-4344-9be3-5d984e0198b4"), "Off-road", new Guid("268e0e46-4505-4cd3-b74a-cf484d98d98c") },
                { new Guid("aef73d6b-4544-4b2c-90d2-46acd83ff47a"), "Formula", new Guid("268e0e46-4505-4cd3-b74a-cf484d98d98c") },
                { new Guid("b59956e8-8ff3-465c-988d-673937f3c0b4"), "TBS", new Guid("f3ed9c55-5a2d-4c3b-8f19-b5ea04e45764") },
                { new Guid("f3e266a3-394b-4737-a14e-e06a51ff9dcd"), "Arcade", new Guid("268e0e46-4505-4cd3-b74a-cf484d98d98c") },
                { new Guid("f3ed9c55-5a2d-4c3b-8f19-b5ea04e45764"), "Strategy", null },
                { new Guid("ff9b9e82-b1b6-4a2e-8467-14a23a55a3cd"), "Sports", null },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("3c2b1528-fa27-450b-b789-4573ed2b3c7c"), "Console" },
                { new Guid("53c519a3-fb44-411f-b180-2e580dd5dd23"), "Browser" },
                { new Guid("641b74bc-55c0-4ade-9e5f-fea6aefffd61"), "Desktop" },
                { new Guid("e0df8119-39d1-4bce-92bf-1f9a15207fa0"), "Mobile" },
            });
    }
}
