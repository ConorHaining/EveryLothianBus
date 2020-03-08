using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryBus.Migrations
{
    public partial class UpdateRouteNameType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StopId",
                table: "Stops",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Stops",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Stops",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Services",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "StopId",
                table: "Stops",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Longitude",
                table: "Stops",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<long>(
                name: "Latitude",
                table: "Stops",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
