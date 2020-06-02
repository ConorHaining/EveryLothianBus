using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryBus.Migrations
{
    public partial class CaptureMoreStopData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AtocCode",
                table: "Stops",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "Stops",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Stops",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Locality",
                table: "Stops",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Stops",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Orientation",
                table: "Stops",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "Stops",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtocCode",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "Locality",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "Orientation",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "Stops");
        }
    }
}
