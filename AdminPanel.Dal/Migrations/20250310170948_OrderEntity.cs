using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class OrderEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_Publisher_PublisherId",
            table: "Games");

        migrationBuilder.DropIndex(
            name: "IX_Games_PublisherId",
            table: "Games");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Publisher",
            table: "Publisher");

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("0f970552-0bc0-4224-8d61-d16992a34f75"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("17821d7c-03a8-44aa-a133-f72a011f022a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("28222329-9745-49fd-ab89-423c00a0c6d3"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("336b2ff0-01c0-417a-9db7-571fc7100522"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("3e516553-dbbd-480f-9ec3-70d86ea0ac85"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("45dc3091-46c3-4624-94a2-6448caaf40ff"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("52a98fc2-7d3d-41f7-9455-0af0a4c85147"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("64cd5e25-c9d0-4603-b645-e881635e1855"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("6749861a-4247-4bb5-90f7-0404a0c3dd5c"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("68712dd6-d179-465c-8745-7564d0b16e79"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a0d25ce7-0bd0-43fd-8c62-c60c2bf8761e"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b09a3bb5-2408-4810-917f-c11b851c923f"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e65f4abe-00f8-4695-bc38-fd5a882cbdd4"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e7d8e3e0-4682-432d-b039-bd6987a1fef7"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ea6f1cdc-3541-4e6d-97b5-d149f3967f19"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("0765f292-7dc3-4635-b49b-a78161245327"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("3b3791d5-9e7c-4b4c-b3e5-62aa5e656218"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("8008c5a5-b846-426c-b058-82c68ebc5634"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("f88014d9-c265-4652-927b-dab01ae7d5ed"));

        migrationBuilder.RenameTable(
            name: "Publisher",
            newName: "Publishers");

        migrationBuilder.RenameIndex(
            name: "IX_Publisher_CompanyName",
            table: "Publishers",
            newName: "IX_Publishers_CompanyName");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Publishers",
            table: "Publishers",
            column: "Id");

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

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Publishers",
            table: "Publishers");

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

        migrationBuilder.RenameTable(
            name: "Publishers",
            newName: "Publisher");

        migrationBuilder.RenameIndex(
            name: "IX_Publishers_CompanyName",
            table: "Publisher",
            newName: "IX_Publisher_CompanyName");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Publisher",
            table: "Publisher",
            column: "Id");

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("0f970552-0bc0-4224-8d61-d16992a34f75"), "FPS", new Guid("336b2ff0-01c0-417a-9db7-571fc7100522") },
                { new Guid("17821d7c-03a8-44aa-a133-f72a011f022a"), "Races", null },
                { new Guid("28222329-9745-49fd-ab89-423c00a0c6d3"), "Strategy", null },
                { new Guid("336b2ff0-01c0-417a-9db7-571fc7100522"), "Action", null },
                { new Guid("3e516553-dbbd-480f-9ec3-70d86ea0ac85"), "Formula", new Guid("17821d7c-03a8-44aa-a133-f72a011f022a") },
                { new Guid("45dc3091-46c3-4624-94a2-6448caaf40ff"), "RTS", new Guid("28222329-9745-49fd-ab89-423c00a0c6d3") },
                { new Guid("52a98fc2-7d3d-41f7-9455-0af0a4c85147"), "Arcade", new Guid("17821d7c-03a8-44aa-a133-f72a011f022a") },
                { new Guid("64cd5e25-c9d0-4603-b645-e881635e1855"), "Adventure", null },
                { new Guid("6749861a-4247-4bb5-90f7-0404a0c3dd5c"), "TPS", new Guid("336b2ff0-01c0-417a-9db7-571fc7100522") },
                { new Guid("68712dd6-d179-465c-8745-7564d0b16e79"), "Rally", new Guid("17821d7c-03a8-44aa-a133-f72a011f022a") },
                { new Guid("a0d25ce7-0bd0-43fd-8c62-c60c2bf8761e"), "RPG", null },
                { new Guid("b09a3bb5-2408-4810-917f-c11b851c923f"), "Puzzle & Skill", null },
                { new Guid("e65f4abe-00f8-4695-bc38-fd5a882cbdd4"), "Sports", null },
                { new Guid("e7d8e3e0-4682-432d-b039-bd6987a1fef7"), "TBS", new Guid("28222329-9745-49fd-ab89-423c00a0c6d3") },
                { new Guid("ea6f1cdc-3541-4e6d-97b5-d149f3967f19"), "Off-road", new Guid("17821d7c-03a8-44aa-a133-f72a011f022a") },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("0765f292-7dc3-4635-b49b-a78161245327"), "Mobile" },
                { new Guid("3b3791d5-9e7c-4b4c-b3e5-62aa5e656218"), "Browser" },
                { new Guid("8008c5a5-b846-426c-b058-82c68ebc5634"), "Desktop" },
                { new Guid("f88014d9-c265-4652-927b-dab01ae7d5ed"), "Console" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_Games_PublisherId",
            table: "Games",
            column: "PublisherId");

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Publisher_PublisherId",
            table: "Games",
            column: "PublisherId",
            principalTable: "Publisher",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
