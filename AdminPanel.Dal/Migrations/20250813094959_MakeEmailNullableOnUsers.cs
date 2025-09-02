using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class MakeEmailNullableOnUsers : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("04a706d7-4985-4761-aee9-d1330ea09d2b"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("0975366f-6627-4aa5-8555-f8f5f524cb5c"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("2e2773b3-0ea8-4ca4-88d0-68880f6ae1a6"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("3b3ed38c-6d5b-4043-83ea-f7844938715c"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("4d21502c-1999-44db-be2b-1b65b24cedcc"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("95a6a297-eea5-432c-9e73-3f37d7dc32ed"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("98339a14-d8ef-4235-89ab-8bddf473049b"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("9a1ced3c-626f-45c6-9fa3-1f907a750cec"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("a4dcf466-d187-4e3e-8439-f4b52e8b89b8"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("b3a02c11-3db3-4d84-89ce-43cb0ed31e3d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("c817736f-ee6b-4996-aced-98bb6abd4c63"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("e61d985b-4c7d-4073-916b-99d31ede5c10"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f0442bc5-3f33-4587-8909-bea6bf9b2172"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("f492db5d-8d9a-4922-be8c-38f0b3fb766a"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("ffa8323d-f920-43c0-88d0-14f9d022c4a6"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("419ffc60-223d-4435-adf6-5b9ae13ea38f"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("cf6cd5c0-d8c3-4dad-8fc4-c19ca8156219"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("e6813602-c1e9-4889-bf45-c9e3dcf2e4ec"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("ebbe7dac-2e8b-42c6-8ac6-15f637604593"));

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "Users",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(256)",
            oldMaxLength: 256);

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

    protected override void Down(MigrationBuilder migrationBuilder)
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

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "Users",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(256)",
            oldMaxLength: 256,
            oldNullable: true);

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("04a706d7-4985-4761-aee9-d1330ea09d2b"), "Formula", new Guid("4d21502c-1999-44db-be2b-1b65b24cedcc") },
                { new Guid("0975366f-6627-4aa5-8555-f8f5f524cb5c"), "RPG", null },
                { new Guid("2e2773b3-0ea8-4ca4-88d0-68880f6ae1a6"), "Sports", null },
                { new Guid("3b3ed38c-6d5b-4043-83ea-f7844938715c"), "Off-road", new Guid("4d21502c-1999-44db-be2b-1b65b24cedcc") },
                { new Guid("4d21502c-1999-44db-be2b-1b65b24cedcc"), "Races", null },
                { new Guid("95a6a297-eea5-432c-9e73-3f37d7dc32ed"), "RTS", new Guid("9a1ced3c-626f-45c6-9fa3-1f907a750cec") },
                { new Guid("98339a14-d8ef-4235-89ab-8bddf473049b"), "Action", null },
                { new Guid("9a1ced3c-626f-45c6-9fa3-1f907a750cec"), "Strategy", null },
                { new Guid("a4dcf466-d187-4e3e-8439-f4b52e8b89b8"), "Adventure", null },
                { new Guid("b3a02c11-3db3-4d84-89ce-43cb0ed31e3d"), "Puzzle & Skill", null },
                { new Guid("c817736f-ee6b-4996-aced-98bb6abd4c63"), "Arcade", new Guid("4d21502c-1999-44db-be2b-1b65b24cedcc") },
                { new Guid("e61d985b-4c7d-4073-916b-99d31ede5c10"), "TPS", new Guid("98339a14-d8ef-4235-89ab-8bddf473049b") },
                { new Guid("f0442bc5-3f33-4587-8909-bea6bf9b2172"), "TBS", new Guid("9a1ced3c-626f-45c6-9fa3-1f907a750cec") },
                { new Guid("f492db5d-8d9a-4922-be8c-38f0b3fb766a"), "FPS", new Guid("98339a14-d8ef-4235-89ab-8bddf473049b") },
                { new Guid("ffa8323d-f920-43c0-88d0-14f9d022c4a6"), "Rally", new Guid("4d21502c-1999-44db-be2b-1b65b24cedcc") },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("419ffc60-223d-4435-adf6-5b9ae13ea38f"), "Browser" },
                { new Guid("cf6cd5c0-d8c3-4dad-8fc4-c19ca8156219"), "Desktop" },
                { new Guid("e6813602-c1e9-4889-bf45-c9e3dcf2e4ec"), "Console" },
                { new Guid("ebbe7dac-2e8b-42c6-8ac6-15f637604593"), "Mobile" },
            });
    }
}
