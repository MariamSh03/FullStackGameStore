using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class AddUniqueConstraintToPublisher : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_PublisherEntity_PublisherId",
            table: "Games");

        migrationBuilder.DropPrimaryKey(
            name: "PK_PublisherEntity",
            table: "PublisherEntity");

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("04341f67-6cb0-4e21-ad8f-23ebbe472c38"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("08976868-9a24-4151-9ebd-74a831fbc601"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("0b16f5b2-b5e6-4a30-9422-0845f75670c8"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("0cdea00c-1854-4eb9-b8ae-58ae868a182d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("0e2468b0-6256-4b2b-889c-96a8b0a3bf86"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("18f3a7fd-6153-4307-8767-d726418a05da"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("2345ceb7-6054-4f20-bf55-dd4d60671555"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("67bf4797-685c-4e31-ac8e-1bc9e804aeff"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("7218732f-48b4-40b8-a6f2-ca38412c23cf"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("87a545b1-d122-4f55-9ed2-bf3aa00a6ea0"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("99673268-bc84-420b-81e3-1579fe4924b1"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("c83856b5-cc5a-4592-8602-39b852631735"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("d43e7905-4687-4bdd-bacc-63c217c98278"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("db16a27d-8a39-485e-8ba6-498a33eaaeb0"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f78a674a-04dd-4eee-8e68-8d71d7ae6a3b"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("01ac193e-6bd2-43f0-9087-38c5b9b3251e"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("2d252d62-7deb-41e1-a948-46b8a1f058ab"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("9b00c04c-be18-4259-8d2e-292648901711"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("a360872e-3f94-494e-bdda-826c95c8efbe"));

        migrationBuilder.RenameTable(
            name: "PublisherEntity",
            newName: "Publishers");

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

        migrationBuilder.CreateIndex(
            name: "IX_Publishers_CompanyName",
            table: "Publishers",
            column: "CompanyName",
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Publishers_PublisherId",
            table: "Games",
            column: "PublisherId",
            principalTable: "Publishers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_Publishers_PublisherId",
            table: "Games");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Publishers",
            table: "Publishers");

        migrationBuilder.DropIndex(
            name: "IX_Publishers_CompanyName",
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
            newName: "PublisherEntity");

        migrationBuilder.AddPrimaryKey(
            name: "PK_PublisherEntity",
            table: "PublisherEntity",
            column: "Id");

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

        migrationBuilder.AddForeignKey(
            name: "FK_Games_PublisherEntity_PublisherId",
            table: "Games",
            column: "PublisherId",
            principalTable: "PublisherEntity",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
