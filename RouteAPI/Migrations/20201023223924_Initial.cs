using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RouteAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    XCoordinate = table.Column<double>(nullable: false),
                    YCoordinate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XCoordinate = table.Column<double>(nullable: false),
                    YCoordinate = table.Column<double>(nullable: false),
                    LineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coordinates_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timetables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(nullable: false),
                    DayType = table.Column<int>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timetables_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineStations",
                columns: table => new
                {
                    LineId = table.Column<int>(nullable: false),
                    StationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineStations", x => new { x.LineId, x.StationId });
                    table.ForeignKey(
                        name: "FK_LineStations_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineStations_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(nullable: false),
                    TimetableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departures_Timetables_TimetableId",
                        column: x => x.TimetableId,
                        principalTable: "Timetables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Lines",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[] { 1, "2", 1 });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Address", "Name", "XCoordinate", "YCoordinate" },
                values: new object[,]
                {
                    { 1, "Bul. Jovana Ducica", "Tri kule", 45.251052000000001, 19.798290999999999 },
                    { 2, "Futoska", "Higijenski zavod", 45.248686999999997, 19.817565999999999 },
                    { 3, "Uspenska", "Centar", 45.254818999999998, 19.841785000000002 }
                });

            migrationBuilder.InsertData(
                table: "Coordinates",
                columns: new[] { "Id", "LineId", "XCoordinate", "YCoordinate" },
                values: new object[,]
                {
                    { 1, 1, 45.248882999999999, 19.791696999999999 },
                    { 2, 1, 45.253169999999997, 19.804231999999999 },
                    { 3, 1, 45.248804999999997, 19.807176999999999 },
                    { 4, 1, 45.247568999999999, 19.807628000000001 },
                    { 5, 1, 45.248596999999997, 19.816578 },
                    { 6, 1, 45.249366999999999, 19.822073 },
                    { 7, 1, 45.249139999999997, 19.830935 },
                    { 8, 1, 45.254322000000002, 19.842607999999998 },
                    { 9, 1, 45.254877, 19.841878999999999 }
                });

            migrationBuilder.InsertData(
                table: "Timetables",
                columns: new[] { "Id", "Active", "DayType", "From", "LineId", "To" },
                values: new object[] { 1, true, 1, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Departures",
                columns: new[] { "Id", "Time", "TimetableId" },
                values: new object[] { 1, new DateTime(2020, 10, 23, 8, 45, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Departures",
                columns: new[] { "Id", "Time", "TimetableId" },
                values: new object[] { 2, new DateTime(2020, 10, 23, 9, 45, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Departures",
                columns: new[] { "Id", "Time", "TimetableId" },
                values: new object[] { 3, new DateTime(2020, 10, 23, 10, 45, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_LineId",
                table: "Coordinates",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Departures_TimetableId",
                table: "Departures",
                column: "TimetableId");

            migrationBuilder.CreateIndex(
                name: "IX_LineStations_StationId",
                table: "LineStations",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_LineId",
                table: "Timetables",
                column: "LineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropTable(
                name: "Departures");

            migrationBuilder.DropTable(
                name: "LineStations");

            migrationBuilder.DropTable(
                name: "Timetables");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "Lines");
        }
    }
}
