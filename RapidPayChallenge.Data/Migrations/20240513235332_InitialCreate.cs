using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RapidPayChallenge.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pass = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymFees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymFees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ExpMonth = table.Column<int>(type: "int", nullable: false),
                    ExpYear = table.Column<int>(type: "int", nullable: false),
                    CVC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacs_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountId",
                table: "Cards",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacs_CardId",
                table: "Transacs",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymFees");

            migrationBuilder.DropTable(
                name: "Transacs");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
