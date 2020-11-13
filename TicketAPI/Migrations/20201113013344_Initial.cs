using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coefficients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserType = table.Column<int>(nullable: false),
                    Value = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coefficients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pricelists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricelists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PricelistItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(nullable: false),
                    PricelistId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricelistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricelistItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PricelistItems_Pricelists_PricelistId",
                        column: x => x.PricelistId,
                        principalTable: "Pricelists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueTime = table.Column<DateTime>(nullable: false),
                    PricelistItemId = table.Column<int>(nullable: false),
                    Valid = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_PricelistItems_PricelistItemId",
                        column: x => x.PricelistItemId,
                        principalTable: "PricelistItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Coefficients",
                columns: new[] { "Id", "UserType", "Value" },
                values: new object[,]
                {
                    { 1, 1, 1.0 },
                    { 2, 2, 0.80000000000000004 },
                    { 3, 3, 0.84999999999999998 }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "TicketType" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "Pricelists",
                columns: new[] { "Id", "Active", "From", "To" },
                values: new object[] { 1, true, new DateTime(2020, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "PricelistItems",
                columns: new[] { "Id", "ItemId", "Price", "PricelistId" },
                values: new object[,]
                {
                    { 1, 1, 70.0, 1 },
                    { 2, 2, 250.0, 1 },
                    { 3, 3, 1600.0, 1 },
                    { 4, 4, 12000.0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "IssueTime", "Price", "PricelistItemId", "UserId", "Valid" },
                values: new object[] { 1, new DateTime(2020, 8, 12, 8, 22, 12, 0, DateTimeKind.Unspecified), 70.0, 1, 3, true });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "IssueTime", "Price", "PricelistItemId", "UserId", "Valid" },
                values: new object[] { 2, new DateTime(2020, 6, 5, 16, 13, 56, 0, DateTimeKind.Unspecified), 250.0, 2, 3, true });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "IssueTime", "Price", "PricelistItemId", "UserId", "Valid" },
                values: new object[] { 3, new DateTime(2020, 11, 12, 10, 13, 0, 0, DateTimeKind.Unspecified), 10000.0, 4, 3, true });

            migrationBuilder.CreateIndex(
                name: "IX_PricelistItems_ItemId",
                table: "PricelistItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PricelistItems_PricelistId",
                table: "PricelistItems",
                column: "PricelistId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PricelistItemId",
                table: "Tickets",
                column: "PricelistItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coefficients");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "PricelistItems");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Pricelists");
        }
    }
}
