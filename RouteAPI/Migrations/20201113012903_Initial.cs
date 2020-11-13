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
                values: new object[,]
                {
                    { 1, "2", 1 },
                    { 2, "3", 1 },
                    { 3, "9", 2 }
                });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Address", "Name", "XCoordinate", "YCoordinate" },
                values: new object[,]
                {
                    { 1, "Bul. Jovana Ducica", "Tri kule", 45.251052000000001, 19.798290999999999 },
                    { 2, "Futoska", "Higijenski zavod", 45.248686999999997, 19.817565999999999 },
                    { 3, "Uspenska", "Centar", 45.254818999999998, 19.841785000000002 },
                    { 4, "Futoska", "Futoski park", 45.249302, 19.828547 },
                    { 5, "Cara Dusana", "Medicinska skola", 45.243614999999998, 19.825137000000002 },
                    { 6, "Bulevar oslobodjenja", "NIS", 45.242125000000001, 19.842656000000002 },
                    { 7, "Bul. oslobodjenja", "Limanski park", 45.241152999999997, 19.842749000000001 },
                    { 8, "Bul. Mihajla Pupina", "Dunavski park", 45.254052000000001, 19.85239 },
                    { 9, "Beogradska", "Kapija", 45.254618999999998, 19.862797 },
                    { 10, "Reljkoviceva", "Petrovaradin", 45.251885000000001, 19.876567999999999 }
                });

            migrationBuilder.InsertData(
                table: "Coordinates",
                columns: new[] { "Id", "LineId", "XCoordinate", "YCoordinate" },
                values: new object[,]
                {
                    { 1, 1, 45.248882999999999, 19.791696999999999 },
                    { 28, 2, 45.237127000000001, 19.826509999999999 },
                    { 29, 3, 45.253379000000002, 19.844736000000001 },
                    { 30, 3, 45.253556000000003, 19.846695 },
                    { 31, 3, 45.253754999999998, 19.848721999999999 },
                    { 32, 3, 45.253852999999999, 19.849674 },
                    { 33, 3, 45.253998000000003, 19.851122 },
                    { 34, 3, 45.254218000000002, 19.853408999999999 },
                    { 35, 3, 45.254398999999999, 19.855025999999999 },
                    { 36, 3, 45.254601000000001, 19.857119999999998 },
                    { 37, 3, 45.254823000000002, 19.859414999999998 },
                    { 27, 2, 45.238531999999999, 19.825835000000001 },
                    { 38, 3, 45.254883999999997, 19.860910000000001 },
                    { 40, 3, 45.253836999999997, 19.865122 },
                    { 41, 3, 45.252794999999999, 19.866892 },
                    { 42, 3, 45.252491999999997, 19.868995000000002 },
                    { 43, 3, 45.252301000000003, 19.871711000000001 },
                    { 44, 3, 45.250781000000003, 19.873906999999999 },
                    { 45, 3, 45.248609000000002, 19.876946 },
                    { 46, 3, 45.250326999999999, 19.876978000000001 },
                    { 47, 3, 45.252735000000001, 19.876224000000001 },
                    { 48, 3, 45.253048, 19.878081999999999 },
                    { 49, 3, 45.253602999999998, 19.881439 },
                    { 39, 3, 45.254584999999999, 19.863603999999999 },
                    { 25, 2, 45.243386000000001, 19.825240000000001 },
                    { 26, 2, 45.239845000000003, 19.825330999999998 },
                    { 23, 2, 45.248145000000001, 19.824997 },
                    { 2, 1, 45.253169999999997, 19.804231999999999 },
                    { 3, 1, 45.248804999999997, 19.807176999999999 },
                    { 4, 1, 45.247568999999999, 19.807628000000001 },
                    { 5, 1, 45.248596999999997, 19.816578 },
                    { 6, 1, 45.249366999999999, 19.822073 },
                    { 7, 1, 45.249139999999997, 19.830935 },
                    { 8, 1, 45.254322000000002, 19.842607999999998 },
                    { 9, 1, 45.254877, 19.841878999999999 },
                    { 10, 2, 45.236991000000003, 19.826449 },
                    { 11, 2, 45.238233000000001, 19.830770999999999 },
                    { 24, 2, 45.245547999999999, 19.825106999999999 },
                    { 13, 2, 45.239604999999997, 19.835805000000001 },
                    { 12, 2, 45.239184999999999, 19.834212999999998 },
                    { 15, 2, 45.244140000000002, 19.841380999999998 },
                    { 16, 2, 45.246419000000003, 19.840147999999999 },
                    { 17, 2, 45.247973999999999, 19.839265000000001 },
                    { 18, 2, 45.24774, 19.836482 },
                    { 19, 2, 45.248657999999999, 19.833655 },
                    { 20, 2, 45.249699999999997, 19.832616000000002 },
                    { 21, 2, 45.249217999999999, 19.830915999999998 },
                    { 22, 2, 45.249294999999996, 19.824555 },
                    { 14, 2, 45.241639999999997, 19.842815999999999 }
                });

            migrationBuilder.InsertData(
                table: "LineStations",
                columns: new[] { "LineId", "StationId" },
                values: new object[,]
                {
                    { 2, 6 },
                    { 3, 8 },
                    { 2, 4 },
                    { 2, 5 },
                    { 1, 3 },
                    { 1, 2 },
                    { 3, 10 },
                    { 3, 9 },
                    { 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Timetables",
                columns: new[] { "Id", "Active", "DayType", "From", "LineId", "To" },
                values: new object[,]
                {
                    { 6, true, 2, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, true, 1, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2021, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, true, 1, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2020, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, true, 2, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, true, 1, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, true, 2, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2020, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Departures",
                columns: new[] { "Id", "Time", "TimetableId" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 10, 23, 8, 45, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2020, 10, 23, 9, 45, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, new DateTime(2020, 10, 23, 10, 45, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 4, new DateTime(2020, 10, 23, 8, 45, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 5, new DateTime(2020, 10, 23, 19, 30, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 6, new DateTime(2020, 10, 23, 8, 45, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 7, new DateTime(2020, 10, 23, 9, 10, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 8, new DateTime(2020, 10, 23, 10, 23, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 9, new DateTime(2020, 10, 23, 12, 45, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 10, new DateTime(2020, 10, 23, 19, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 11, new DateTime(2020, 10, 23, 8, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 12, new DateTime(2020, 10, 23, 10, 20, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 13, new DateTime(2020, 10, 23, 14, 45, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 14, new DateTime(2020, 10, 23, 20, 37, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 15, new DateTime(2020, 10, 23, 13, 0, 0, 0, DateTimeKind.Unspecified), 6 }
                });

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
