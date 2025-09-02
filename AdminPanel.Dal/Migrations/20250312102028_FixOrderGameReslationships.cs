using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class FixOrderGameReslationships : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("15e5a8b5-7f03-4716-b6ac-272d13e2219f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("34a3c1a7-2004-428e-a5f8-b1051d7a085e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("36a5f673-7632-448b-9fc9-8bbee42bd57e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("3dc11065-48ab-40de-8f75-ffaa6a508f34"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("4c362962-0044-43d7-be43-a8dda3b5871f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("650b24b9-c996-4ff6-85fe-b07cf5b024ab"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("6948d794-d33d-4fdb-8eae-77b290348a2f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("7900d574-8f10-4b60-b5fe-7ee84a2c3b93"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("7edfd4e6-51a6-4d00-8ae6-8df63cba5847"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ab5758f5-8b9d-4b3f-909a-f46ac919a203"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b1c98338-848c-46ee-97f0-9640cee28df5"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("c523a190-26e0-4731-85bf-a46c28645cc8"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e8b5e9a3-3eac-4778-8cf6-7637d92b0fa7"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ebd1868c-f848-4bd7-83b0-3bc222a0d1b8"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ed0874ba-6067-4e83-b45a-043618f5575a"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("2e5d8ae0-9d0a-4e07-ab2b-6abcbf4d93af"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("316f85c6-8cc4-408e-b85d-515da326325f"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("a5426a23-a715-455b-b5ed-7166f72e9264"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("e4cedd08-819c-42ea-b902-5aa97c343d2b"));

        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OrderGames",
            columns: table => new
            {
                OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Price = table.Column<double>(type: "float", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                Discount = table.Column<int>(type: "int", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderGames", x => new { x.OrderId, x.ProductId });
                table.ForeignKey(
                    name: "FK_OrderGames_Games_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_OrderGames_Orders_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Orders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

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

        migrationBuilder.CreateIndex(
            name: "IX_OrderGames_ProductId",
            table: "OrderGames",
            column: "ProductId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OrderGames");

        migrationBuilder.DropTable(
            name: "Orders");

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

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("15e5a8b5-7f03-4716-b6ac-272d13e2219f"), "TBS", new Guid("4c362962-0044-43d7-be43-a8dda3b5871f") },
                { new Guid("34a3c1a7-2004-428e-a5f8-b1051d7a085e"), "Rally", new Guid("c523a190-26e0-4731-85bf-a46c28645cc8") },
                { new Guid("36a5f673-7632-448b-9fc9-8bbee42bd57e"), "Sports", null },
                { new Guid("3dc11065-48ab-40de-8f75-ffaa6a508f34"), "FPS", new Guid("7900d574-8f10-4b60-b5fe-7ee84a2c3b93") },
                { new Guid("4c362962-0044-43d7-be43-a8dda3b5871f"), "Strategy", null },
                { new Guid("650b24b9-c996-4ff6-85fe-b07cf5b024ab"), "Puzzle & Skill", null },
                { new Guid("6948d794-d33d-4fdb-8eae-77b290348a2f"), "Off-road", new Guid("c523a190-26e0-4731-85bf-a46c28645cc8") },
                { new Guid("7900d574-8f10-4b60-b5fe-7ee84a2c3b93"), "Action", null },
                { new Guid("7edfd4e6-51a6-4d00-8ae6-8df63cba5847"), "RTS", new Guid("4c362962-0044-43d7-be43-a8dda3b5871f") },
                { new Guid("ab5758f5-8b9d-4b3f-909a-f46ac919a203"), "Adventure", null },
                { new Guid("b1c98338-848c-46ee-97f0-9640cee28df5"), "Arcade", new Guid("c523a190-26e0-4731-85bf-a46c28645cc8") },
                { new Guid("c523a190-26e0-4731-85bf-a46c28645cc8"), "Races", null },
                { new Guid("e8b5e9a3-3eac-4778-8cf6-7637d92b0fa7"), "TPS", new Guid("7900d574-8f10-4b60-b5fe-7ee84a2c3b93") },
                { new Guid("ebd1868c-f848-4bd7-83b0-3bc222a0d1b8"), "RPG", null },
                { new Guid("ed0874ba-6067-4e83-b45a-043618f5575a"), "Formula", new Guid("c523a190-26e0-4731-85bf-a46c28645cc8") },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("2e5d8ae0-9d0a-4e07-ab2b-6abcbf4d93af"), "Desktop" },
                { new Guid("316f85c6-8cc4-408e-b85d-515da326325f"), "Mobile" },
                { new Guid("a5426a23-a715-455b-b5ed-7166f72e9264"), "Console" },
                { new Guid("e4cedd08-819c-42ea-b902-5aa97c343d2b"), "Browser" },
            });
    }
}
