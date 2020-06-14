using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryBus.Migrations
{
    public partial class CreateRouteStopJoiningTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Routes_RouteId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_RouteId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Stops");

            migrationBuilder.CreateTable(
                name: "RouteStop",
                columns: table => new
                {
                    Order = table.Column<int>(nullable: false),
                    RouteId = table.Column<Guid>(nullable: false),
                    StopId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStop", x => new { x.Order, x.RouteId, x.StopId });
                    table.ForeignKey(
                        name: "FK_RouteStop_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RouteStop_Stops_StopId",
                        column: x => x.StopId,
                        principalTable: "Stops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteStop_RouteId",
                table: "RouteStop",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStop_StopId",
                table: "RouteStop",
                column: "StopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteStop");

            migrationBuilder.AddColumn<string>(
                name: "RouteId",
                table: "Stops",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_RouteId",
                table: "Stops",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Routes_RouteId",
                table: "Stops",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
