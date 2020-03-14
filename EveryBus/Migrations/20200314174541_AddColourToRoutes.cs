using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryBus.Migrations
{
    public partial class AddColourToRoutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Services",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextColor",
                table: "Services",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "TextColor",
                table: "Services");
        }
    }
}
