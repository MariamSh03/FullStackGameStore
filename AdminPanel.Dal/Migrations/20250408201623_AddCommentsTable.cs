using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class AddCommentsTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("2d365341-7b45-464e-8058-0623d96f9091"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("4eb95111-b8e8-4f89-bf26-95255bece679"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("532e0b70-4523-4490-ba46-6b216232eb23"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("55d4da04-cfc0-477d-a48e-6385174690f4"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("6af88aea-5fe3-4640-9270-b738b9cfeac6"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("7262a5f6-4b44-4222-ad58-0b3c88c2353d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("75da4cb3-ba16-484d-80e1-0b422128da40"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("95b3ee92-a81c-4391-b832-ccd4ddd49970"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("96fb31b7-5b80-43bf-87b2-22804a30648e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("9eb241c0-6b2e-4bd5-9748-c371db66afd1"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("9f45ab7c-0d81-4642-bdc9-7407297eff8e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a04ccd99-70fb-4e16-9b5f-c3d103fea09a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("acb7bc68-553d-4112-83a0-cc790d954a98"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("bcbaa995-55b5-410e-82bf-27ae0efec6b4"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("c3077886-4d17-444e-912c-0e410f2332c6"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("05ded520-6e1c-4279-b552-33d8f46fdeb3"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("134cebba-c4df-4b6c-a085-177de998be29"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("4d5dfb11-d358-47f8-b9bb-a326d8a95daa"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("e5269c77-72d1-40ef-8175-03f038c56c1d"));

        migrationBuilder.CreateTable(
            name: "Comments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ParentCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Comments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Comments_Comments_ParentCommentId",
                    column: x => x.ParentCommentId,
                    principalTable: "Comments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Comments_Games_GameId",
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
                { new Guid("0f9d5798-8f49-4400-b6bc-428eb5bbf2f4"), "Races", null },
                { new Guid("13208af4-f67c-4f6c-abda-896dce2f06cc"), "RPG", null },
                { new Guid("13caa215-a095-431f-a569-73ece301c7cd"), "TBS", new Guid("750e31ff-ba3b-4d0d-9745-9227a30f7fa4") },
                { new Guid("26e97eab-86b2-4a19-8ca7-001ec0f0347e"), "Formula", new Guid("0f9d5798-8f49-4400-b6bc-428eb5bbf2f4") },
                { new Guid("439dddb9-0fbf-4f3c-ab21-0440d7d9043d"), "FPS", new Guid("8c4424e3-20cc-4910-ac28-de77315f7645") },
                { new Guid("51006abd-4462-4e06-acb9-4ffd1836504a"), "RTS", new Guid("750e31ff-ba3b-4d0d-9745-9227a30f7fa4") },
                { new Guid("5ba346d6-0731-46dd-bcd9-b7e121fc0295"), "TPS", new Guid("8c4424e3-20cc-4910-ac28-de77315f7645") },
                { new Guid("750e31ff-ba3b-4d0d-9745-9227a30f7fa4"), "Strategy", null },
                { new Guid("7a54dfff-8b76-41cb-86af-4f0917321422"), "Off-road", new Guid("0f9d5798-8f49-4400-b6bc-428eb5bbf2f4") },
                { new Guid("80918149-2f6d-4aad-a94b-2bd70690d07d"), "Arcade", new Guid("0f9d5798-8f49-4400-b6bc-428eb5bbf2f4") },
                { new Guid("8c4424e3-20cc-4910-ac28-de77315f7645"), "Action", null },
                { new Guid("c74c6fa0-214a-4eee-b872-de844df2905f"), "Sports", null },
                { new Guid("d80f1f6a-3e61-4700-b899-f564024e5617"), "Puzzle & Skill", null },
                { new Guid("db6ababe-b372-41d8-8d4a-fc9cac89237a"), "Rally", new Guid("0f9d5798-8f49-4400-b6bc-428eb5bbf2f4") },
                { new Guid("fa335d4c-3d6e-4b39-a188-87a2a3f67724"), "Adventure", null },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("5a26768b-dbd7-40c5-9219-808afa6a956c"), "Browser" },
                { new Guid("6a22f90d-ce8a-4883-a870-35b7aac9594c"), "Desktop" },
                { new Guid("82fb2171-7e3a-48a8-86e8-13c2961ffbd3"), "Console" },
                { new Guid("883be2b5-b860-42ac-9ff8-e7868db91a20"), "Mobile" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_Comments_GameId",
            table: "Comments",
            column: "GameId");

        migrationBuilder.CreateIndex(
            name: "IX_Comments_ParentCommentId",
            table: "Comments",
            column: "ParentCommentId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Comments");

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("0f9d5798-8f49-4400-b6bc-428eb5bbf2f4"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("13208af4-f67c-4f6c-abda-896dce2f06cc"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("13caa215-a095-431f-a569-73ece301c7cd"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("26e97eab-86b2-4a19-8ca7-001ec0f0347e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("439dddb9-0fbf-4f3c-ab21-0440d7d9043d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("51006abd-4462-4e06-acb9-4ffd1836504a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5ba346d6-0731-46dd-bcd9-b7e121fc0295"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("750e31ff-ba3b-4d0d-9745-9227a30f7fa4"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("7a54dfff-8b76-41cb-86af-4f0917321422"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("80918149-2f6d-4aad-a94b-2bd70690d07d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("8c4424e3-20cc-4910-ac28-de77315f7645"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("c74c6fa0-214a-4eee-b872-de844df2905f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d80f1f6a-3e61-4700-b899-f564024e5617"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("db6ababe-b372-41d8-8d4a-fc9cac89237a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("fa335d4c-3d6e-4b39-a188-87a2a3f67724"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("5a26768b-dbd7-40c5-9219-808afa6a956c"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("6a22f90d-ce8a-4883-a870-35b7aac9594c"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("82fb2171-7e3a-48a8-86e8-13c2961ffbd3"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("883be2b5-b860-42ac-9ff8-e7868db91a20"));

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("2d365341-7b45-464e-8058-0623d96f9091"), "TPS", new Guid("9eb241c0-6b2e-4bd5-9748-c371db66afd1") },
                { new Guid("4eb95111-b8e8-4f89-bf26-95255bece679"), "Formula", new Guid("a04ccd99-70fb-4e16-9b5f-c3d103fea09a") },
                { new Guid("532e0b70-4523-4490-ba46-6b216232eb23"), "Puzzle & Skill", null },
                { new Guid("55d4da04-cfc0-477d-a48e-6385174690f4"), "Rally", new Guid("a04ccd99-70fb-4e16-9b5f-c3d103fea09a") },
                { new Guid("6af88aea-5fe3-4640-9270-b738b9cfeac6"), "TBS", new Guid("9f45ab7c-0d81-4642-bdc9-7407297eff8e") },
                { new Guid("7262a5f6-4b44-4222-ad58-0b3c88c2353d"), "RTS", new Guid("9f45ab7c-0d81-4642-bdc9-7407297eff8e") },
                { new Guid("75da4cb3-ba16-484d-80e1-0b422128da40"), "RPG", null },
                { new Guid("95b3ee92-a81c-4391-b832-ccd4ddd49970"), "Adventure", null },
                { new Guid("96fb31b7-5b80-43bf-87b2-22804a30648e"), "Sports", null },
                { new Guid("9eb241c0-6b2e-4bd5-9748-c371db66afd1"), "Action", null },
                { new Guid("9f45ab7c-0d81-4642-bdc9-7407297eff8e"), "Strategy", null },
                { new Guid("a04ccd99-70fb-4e16-9b5f-c3d103fea09a"), "Races", null },
                { new Guid("acb7bc68-553d-4112-83a0-cc790d954a98"), "Arcade", new Guid("a04ccd99-70fb-4e16-9b5f-c3d103fea09a") },
                { new Guid("bcbaa995-55b5-410e-82bf-27ae0efec6b4"), "FPS", new Guid("9eb241c0-6b2e-4bd5-9748-c371db66afd1") },
                { new Guid("c3077886-4d17-444e-912c-0e410f2332c6"), "Off-road", new Guid("a04ccd99-70fb-4e16-9b5f-c3d103fea09a") },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("05ded520-6e1c-4279-b552-33d8f46fdeb3"), "Browser" },
                { new Guid("134cebba-c4df-4b6c-a085-177de998be29"), "Mobile" },
                { new Guid("4d5dfb11-d358-47f8-b9bb-a326d8a95daa"), "Desktop" },
                { new Guid("e5269c77-72d1-40ef-8175-03f038c56c1d"), "Console" },
            });
    }
}
