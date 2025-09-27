using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class AddGameLocalizationTable : Migration
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
                Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("0e4e92f9-d1c8-4f6f-94d8-303f5963503a"), "Arcade", new Guid("cc0e0667-4c0a-4576-82bf-121d75000056") },
                { new Guid("11590e1f-a909-4fbb-901b-5d40a396fc8e"), "RPG", null },
                { new Guid("33e8641c-2b26-4616-b5a2-d6d2bbb7bfea"), "FPS", new Guid("a7bfd5c4-e312-4c05-a57b-1998467c1e64") },
                { new Guid("3c37a905-4ddc-41c4-b46c-e602d7aa9430"), "Sports", null },
                { new Guid("53d2b5ce-b0f8-4de0-b3f8-0b1adb4af376"), "TBS", new Guid("797a50a4-06e3-4a3c-abe8-6067cf64f266") },
                { new Guid("6fd9bc50-2210-475f-9342-f65d6d9b2214"), "Formula", new Guid("cc0e0667-4c0a-4576-82bf-121d75000056") },
                { new Guid("797a50a4-06e3-4a3c-abe8-6067cf64f266"), "Strategy", null },
                { new Guid("923418f8-34de-4cc0-a221-0286eb0ed965"), "Adventure", null },
                { new Guid("9b021594-754c-47db-beae-44a5dc1caee9"), "Off-road", new Guid("cc0e0667-4c0a-4576-82bf-121d75000056") },
                { new Guid("a047e8e1-1b01-4798-bc2a-f1e22ce0f2fb"), "RTS", new Guid("797a50a4-06e3-4a3c-abe8-6067cf64f266") },
                { new Guid("a7bfd5c4-e312-4c05-a57b-1998467c1e64"), "Action", null },
                { new Guid("c2c80d00-2fdd-456c-9b6f-2d77e1b1a34a"), "TPS", new Guid("a7bfd5c4-e312-4c05-a57b-1998467c1e64") },
                { new Guid("cc0e0667-4c0a-4576-82bf-121d75000056"), "Races", null },
                { new Guid("d4fcaf4b-b858-4fd4-bae6-cbf6e203386a"), "Rally", new Guid("cc0e0667-4c0a-4576-82bf-121d75000056") },
                { new Guid("fa374ea1-8824-4b92-be12-b747146bf6e7"), "Puzzle & Skill", null },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("13634dd5-660c-4038-89e1-493f69c5012a"), "Mobile" },
                { new Guid("d43e7e81-9337-4a33-8d07-b736fe4966a3"), "Browser" },
                { new Guid("d70fc2ce-7a48-48a2-865d-8350f9369152"), "Desktop" },
                { new Guid("e207b4a2-1292-4950-ad76-29eca9a95373"), "Console" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameLocalizations_GameId_Language",
            table: "GameLocalizations",
            columns: new[] { "GameId", "Language" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameLocalizations");

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("0e4e92f9-d1c8-4f6f-94d8-303f5963503a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11590e1f-a909-4fbb-901b-5d40a396fc8e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("33e8641c-2b26-4616-b5a2-d6d2bbb7bfea"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("3c37a905-4ddc-41c4-b46c-e602d7aa9430"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("53d2b5ce-b0f8-4de0-b3f8-0b1adb4af376"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("6fd9bc50-2210-475f-9342-f65d6d9b2214"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("797a50a4-06e3-4a3c-abe8-6067cf64f266"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("923418f8-34de-4cc0-a221-0286eb0ed965"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("9b021594-754c-47db-beae-44a5dc1caee9"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a047e8e1-1b01-4798-bc2a-f1e22ce0f2fb"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a7bfd5c4-e312-4c05-a57b-1998467c1e64"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("c2c80d00-2fdd-456c-9b6f-2d77e1b1a34a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("cc0e0667-4c0a-4576-82bf-121d75000056"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d4fcaf4b-b858-4fd4-bae6-cbf6e203386a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("fa374ea1-8824-4b92-be12-b747146bf6e7"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("13634dd5-660c-4038-89e1-493f69c5012a"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("d43e7e81-9337-4a33-8d07-b736fe4966a3"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("d70fc2ce-7a48-48a2-865d-8350f9369152"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("e207b4a2-1292-4950-ad76-29eca9a95373"));

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
