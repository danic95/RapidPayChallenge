using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RapidPayChallenge.Data.Migrations
{
    public partial class AddAccountIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Accounts_AccountId",
                table: "Cards");

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("96b367bf-a0c9-41e0-9ae6-3d926b0d9e79"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("9cd80e88-9646-44b7-ac64-62238f4e13ec"));

            migrationBuilder.DeleteData(
                table: "PaymFees",
                keyColumn: "Id",
                keyValue: new Guid("10a3d83b-67d2-498d-b535-6a668949da4f"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("05b23ed2-4883-4e09-8ae0-00edb02818eb"));

            migrationBuilder.RenameColumn(
                name: "Pass",
                table: "Accounts",
                newName: "PasswordHash");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Cards",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEndDateUtc",
                table: "Accounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityUserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityUserClaim_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserLogin<string>",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_IdentityUserLogin<string>_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserRole<string>",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_IdentityUserRole<string>_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccessFailedCount", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEndDateUtc", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d386bcee-d12d-4ae1-83b0-a6bbc0b33c26", 0, "admin@rapidpay.com", false, "Admin", "RapidPay", false, null, "$2a$10$KonwRsnd.SQQWStdYLLwQuxrzcILvNE8gBV1GDHXTIRAQ180Dygl6", null, false, null, false, null });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccessFailedCount", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEndDateUtc", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0c032e66-fbcc-49dc-a9f8-34ca3b31c035", 0, "dcarias@rapidpay.com", false, "Daniel", "Carias", false, null, "$2a$10$CnMrkPCKsAuPFfSCiiGr4.Gmn1f0c4JMulLYOFUGxnlT1G/i/QPGG", null, false, null, false, null });

            migrationBuilder.InsertData(
                table: "PaymFees",
                columns: new[] { "Id", "Created", "Fee" },
                values: new object[] { new Guid("65c176fd-debe-4b1c-ab2c-26e304762f80"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.517305573689428m });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "AccountId", "Balance", "CVC", "ExpMonth", "ExpYear", "Number" },
                values: new object[] { new Guid("a22a7415-03cf-4097-9707-8a8b6f2f888d"), "0c032e66-fbcc-49dc-a9f8-34ca3b31c035", 10000m, "481", 12, 2026, "4228567262279934" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserClaim_AccountId",
                table: "IdentityUserClaim",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserLogin<string>_AccountId",
                table: "IdentityUserLogin<string>",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserRole<string>_AccountId",
                table: "IdentityUserRole<string>",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Accounts_AccountId",
                table: "Cards",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Accounts_AccountId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "IdentityUserClaim");

            migrationBuilder.DropTable(
                name: "IdentityUserLogin<string>");

            migrationBuilder.DropTable(
                name: "IdentityUserRole<string>");

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "d386bcee-d12d-4ae1-83b0-a6bbc0b33c26");

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("a22a7415-03cf-4097-9707-8a8b6f2f888d"));

            migrationBuilder.DeleteData(
                table: "PaymFees",
                keyColumn: "Id",
                keyValue: new Guid("65c176fd-debe-4b1c-ab2c-26e304762f80"));

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: "0c032e66-fbcc-49dc-a9f8-34ca3b31c035");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LockoutEndDateUtc",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Accounts",
                newName: "Pass");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Pass" },
                values: new object[] { new Guid("96b367bf-a0c9-41e0-9ae6-3d926b0d9e79"), "admin@rapidpay.com", "Admin", "RapidPay", "$2a$10$uhdpoXj4ruSD4Zh8DbURJuAqVM4LIs2oLCo5Bej31462lqr7tegWm" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Pass" },
                values: new object[] { new Guid("05b23ed2-4883-4e09-8ae0-00edb02818eb"), "dcarias@rapidpay.com", "Daniel", "Carias", "$2a$10$cwsIm3osBPHVtApOf4IV8eYLiYDELtklykGAvqlJrca/KZ29LWNOG" });

            migrationBuilder.InsertData(
                table: "PaymFees",
                columns: new[] { "Id", "Created", "Fee" },
                values: new object[] { new Guid("10a3d83b-67d2-498d-b535-6a668949da4f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0.852918506065811m });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "AccountId", "Balance", "CVC", "ExpMonth", "ExpYear", "Number" },
                values: new object[] { new Guid("9cd80e88-9646-44b7-ac64-62238f4e13ec"), new Guid("05b23ed2-4883-4e09-8ae0-00edb02818eb"), 10000m, "481", 12, 2026, "4228567262279934" });

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Accounts_AccountId",
                table: "Cards",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
