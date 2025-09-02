using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class RenamePublisherEntityTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_Publishers_PublisherId",
            table: "Games");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Publishers",
            table: "Publishers");

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("241bd8b3-def4-4248-9255-bdfa8858c438"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("33d83330-4112-411c-b821-f169cd26cf3b"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("468b66e1-d113-4fed-b2ac-cc3ebe6eaea9"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("59c14b59-f750-4483-aa65-04cfc2e28590"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("5eaf93a5-9e3d-4ec5-8f86-221e404639e2"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("69673772-1931-4edb-9a05-488f63d13d39"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("887921a6-efbc-4003-9c66-0b07452ee293"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b543fc39-acb1-45d3-9244-7bf86e6807ab"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b8f514b2-e2ed-4644-b1b9-20af6374d6fa"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("bbb001f3-509d-4606-81fb-3eaf29725258"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d2e3bc4b-8e93-4f64-933b-6ecc0ab7c1bc"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e77cc1b1-c329-44f2-8629-018d63755b98"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e846e483-d6b1-4f2c-9563-31aa0f0b52cf"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ebec8090-3810-48b6-9fa7-cc651034a810"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f09e8217-fc79-47ad-add4-cb4ff783160c"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("49dbcef8-4605-4cc1-b470-f800c487eb7c"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("a18f33c4-0fa4-4cad-b36f-61022f0da79b"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("bcec7d01-b33d-4490-a01c-cf10520d8aa7"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("daac496d-b203-4a6a-b67a-720ec4ae4483"));

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

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Publisher_PublisherId",
            table: "Games",
            column: "PublisherId",
            principalTable: "Publisher",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_Publisher_PublisherId",
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
                { new Guid("241bd8b3-def4-4248-9255-bdfa8858c438"), "Action", null },
                { new Guid("33d83330-4112-411c-b821-f169cd26cf3b"), "TBS", new Guid("887921a6-efbc-4003-9c66-0b07452ee293") },
                { new Guid("468b66e1-d113-4fed-b2ac-cc3ebe6eaea9"), "RPG", null },
                { new Guid("59c14b59-f750-4483-aa65-04cfc2e28590"), "Off-road", new Guid("d2e3bc4b-8e93-4f64-933b-6ecc0ab7c1bc") },
                { new Guid("5eaf93a5-9e3d-4ec5-8f86-221e404639e2"), "Rally", new Guid("d2e3bc4b-8e93-4f64-933b-6ecc0ab7c1bc") },
                { new Guid("69673772-1931-4edb-9a05-488f63d13d39"), "RTS", new Guid("887921a6-efbc-4003-9c66-0b07452ee293") },
                { new Guid("887921a6-efbc-4003-9c66-0b07452ee293"), "Strategy", null },
                { new Guid("b543fc39-acb1-45d3-9244-7bf86e6807ab"), "Adventure", null },
                { new Guid("b8f514b2-e2ed-4644-b1b9-20af6374d6fa"), "Puzzle & Skill", null },
                { new Guid("bbb001f3-509d-4606-81fb-3eaf29725258"), "Arcade", new Guid("d2e3bc4b-8e93-4f64-933b-6ecc0ab7c1bc") },
                { new Guid("d2e3bc4b-8e93-4f64-933b-6ecc0ab7c1bc"), "Races", null },
                { new Guid("e77cc1b1-c329-44f2-8629-018d63755b98"), "Sports", null },
                { new Guid("e846e483-d6b1-4f2c-9563-31aa0f0b52cf"), "FPS", new Guid("241bd8b3-def4-4248-9255-bdfa8858c438") },
                { new Guid("ebec8090-3810-48b6-9fa7-cc651034a810"), "TPS", new Guid("241bd8b3-def4-4248-9255-bdfa8858c438") },
                { new Guid("f09e8217-fc79-47ad-add4-cb4ff783160c"), "Formula", new Guid("d2e3bc4b-8e93-4f64-933b-6ecc0ab7c1bc") },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("49dbcef8-4605-4cc1-b470-f800c487eb7c"), "Desktop" },
                { new Guid("a18f33c4-0fa4-4cad-b36f-61022f0da79b"), "Console" },
                { new Guid("bcec7d01-b33d-4490-a01c-cf10520d8aa7"), "Browser" },
                { new Guid("daac496d-b203-4a6a-b67a-720ec4ae4483"), "Mobile" },
            });

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Publishers_PublisherId",
            table: "Games",
            column: "PublisherId",
            principalTable: "Publisher",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
