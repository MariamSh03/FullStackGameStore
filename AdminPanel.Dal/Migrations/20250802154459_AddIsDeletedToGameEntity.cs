using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class AddIsDeletedToGameEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("11d60e2f-f694-4f1d-9459-5de2f8c92c80"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("164bee52-34c4-425f-9145-725fffcc56dc"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("166cbc42-c453-4dff-9a92-5765d70a2ea9"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("2cf1a89e-ccb5-4f39-85bf-2e5522287881"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("2edccd7f-08a7-478e-89f2-4b1ca989d284"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("3212e38c-30c0-4869-93ce-fb46114942d6"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("4f6fdbb5-4068-4ae8-8462-a0c66de00bf9"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("896c0820-a9dc-4aab-b23c-4840864c4e4d"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("8eae2b5c-3025-442d-8891-607396d10543"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("8f244469-d392-4910-8648-d3d5970f3927"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("9811f7ad-c2f3-4c06-88cc-0a8ff654b7c5"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("9859da30-1e54-400c-84a3-e98892c35c37"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("997abe5a-d0b4-4c3c-8696-f3b4eb2d45fd"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("c618b04d-1c8b-482d-a113-a61d00a755aa"));

        migrationBuilder.DeleteData(
            table: "Genres",
            keyColumn: "Id",
            keyValue: new Guid("cf5c2526-5e40-464f-a8d1-70900df1e57a"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("381d34da-ddbb-4860-b2ee-c76c1034e840"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("8da48a53-0469-4609-bbd8-9b1b4d3957e1"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("95d424a4-6285-43b9-8f0b-d4d1e88d67f2"));

        migrationBuilder.DeleteData(
            table: "Platforms",
            keyColumn: "Id",
            keyValue: new Guid("f14f94a9-f74e-4e02-81e8-2d8f8c3ed457"));

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Games",
            type: "bit",
            nullable: false,
            defaultValue: false);

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

    protected override void Down(MigrationBuilder migrationBuilder)
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

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Games");

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("11d60e2f-f694-4f1d-9459-5de2f8c92c80"), "Adventure", null },
                { new Guid("164bee52-34c4-425f-9145-725fffcc56dc"), "FPS", new Guid("997abe5a-d0b4-4c3c-8696-f3b4eb2d45fd") },
                { new Guid("166cbc42-c453-4dff-9a92-5765d70a2ea9"), "TBS", new Guid("9859da30-1e54-400c-84a3-e98892c35c37") },
                { new Guid("2cf1a89e-ccb5-4f39-85bf-2e5522287881"), "RPG", null },
                { new Guid("2edccd7f-08a7-478e-89f2-4b1ca989d284"), "Off-road", new Guid("8eae2b5c-3025-442d-8891-607396d10543") },
                { new Guid("3212e38c-30c0-4869-93ce-fb46114942d6"), "TPS", new Guid("997abe5a-d0b4-4c3c-8696-f3b4eb2d45fd") },
                { new Guid("4f6fdbb5-4068-4ae8-8462-a0c66de00bf9"), "Puzzle & Skill", null },
                { new Guid("896c0820-a9dc-4aab-b23c-4840864c4e4d"), "Formula", new Guid("8eae2b5c-3025-442d-8891-607396d10543") },
                { new Guid("8eae2b5c-3025-442d-8891-607396d10543"), "Races", null },
                { new Guid("8f244469-d392-4910-8648-d3d5970f3927"), "Rally", new Guid("8eae2b5c-3025-442d-8891-607396d10543") },
                { new Guid("9811f7ad-c2f3-4c06-88cc-0a8ff654b7c5"), "Arcade", new Guid("8eae2b5c-3025-442d-8891-607396d10543") },
                { new Guid("9859da30-1e54-400c-84a3-e98892c35c37"), "Strategy", null },
                { new Guid("997abe5a-d0b4-4c3c-8696-f3b4eb2d45fd"), "Action", null },
                { new Guid("c618b04d-1c8b-482d-a113-a61d00a755aa"), "Sports", null },
                { new Guid("cf5c2526-5e40-464f-a8d1-70900df1e57a"), "RTS", new Guid("9859da30-1e54-400c-84a3-e98892c35c37") },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("381d34da-ddbb-4860-b2ee-c76c1034e840"), "Mobile" },
                { new Guid("8da48a53-0469-4609-bbd8-9b1b4d3957e1"), "Console" },
                { new Guid("95d424a4-6285-43b9-8f0b-d4d1e88d67f2"), "Desktop" },
                { new Guid("f14f94a9-f74e-4e02-81e8-2d8f8c3ed457"), "Browser" },
            });
    }
}
