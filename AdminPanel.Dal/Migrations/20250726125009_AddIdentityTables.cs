using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class AddIdentityTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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

        migrationBuilder.CreateTable(
            name: "Permissions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                IsExternalUser = table.Column<bool>(type: "bit", nullable: false),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_RoleClaims_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RolePermissions",
            columns: table => new
            {
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                table.ForeignKey(
                    name: "FK_RolePermissions_Permissions_PermissionId",
                    column: x => x.PermissionId,
                    principalTable: "Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RolePermissions_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserClaims_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_UserLogins_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRoles",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_UserRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRoles_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserTokens",
            columns: table => new
            {
                UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_UserTokens_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

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

        migrationBuilder.CreateIndex(
            name: "IX_RoleClaims_RoleId",
            table: "RoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_RolePermissions_PermissionId",
            table: "RolePermissions",
            column: "PermissionId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "Roles",
            column: "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_UserClaims_UserId",
            table: "UserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserLogins_UserId",
            table: "UserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId",
            table: "UserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "Users",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "Users",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RoleClaims");

        migrationBuilder.DropTable(
            name: "RolePermissions");

        migrationBuilder.DropTable(
            name: "UserClaims");

        migrationBuilder.DropTable(
            name: "UserLogins");

        migrationBuilder.DropTable(
            name: "UserRoles");

        migrationBuilder.DropTable(
            name: "UserTokens");

        migrationBuilder.DropTable(
            name: "Permissions");

        migrationBuilder.DropTable(
            name: "Roles");

        migrationBuilder.DropTable(
            name: "Users");

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
    }
}
