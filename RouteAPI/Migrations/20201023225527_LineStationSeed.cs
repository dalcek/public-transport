using Microsoft.EntityFrameworkCore.Migrations;

namespace RouteAPI.Migrations
{
    public partial class LineStationSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LineStations",
                columns: new[] { "LineId", "StationId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "LineStations",
                columns: new[] { "LineId", "StationId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "LineStations",
                columns: new[] { "LineId", "StationId" },
                values: new object[] { 1, 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LineStations",
                keyColumns: new[] { "LineId", "StationId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "LineStations",
                keyColumns: new[] { "LineId", "StationId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "LineStations",
                keyColumns: new[] { "LineId", "StationId" },
                keyValues: new object[] { 1, 3 });
        }
    }
}
