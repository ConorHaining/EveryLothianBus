using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryBus.Migrations
{
    public partial class AddLastGpsFixAsIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VehicleLocations_LastGpsFix",
                table: "VehicleLocations",
                column: "LastGpsFix");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleLocations_LastGpsFix",
                table: "VehicleLocations");
        }
    }
}
