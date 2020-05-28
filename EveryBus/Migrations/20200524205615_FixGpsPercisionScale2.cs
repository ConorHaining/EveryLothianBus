using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryBus.Migrations
{
    public partial class FixGpsPercisionScale2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Stops",
                type: "decimal(9, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Stops",
                type: "decimal(9, 6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 5)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Stops",
                type: "decimal(8, 5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9, 6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Stops",
                type: "decimal(8, 5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9, 6)");
        }
    }
}
