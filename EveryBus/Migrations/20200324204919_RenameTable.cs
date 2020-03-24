using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EveryBus.Migrations
{
    public partial class RenameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Services_BusServicesId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_BusServicesId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "BusServicesId",
                table: "Routes");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "Routes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ServiceId",
                table: "Routes",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Services_ServiceId",
                table: "Routes",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Services_ServiceId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ServiceId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Routes");

            migrationBuilder.AddColumn<Guid>(
                name: "BusServicesId",
                table: "Routes",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_BusServicesId",
                table: "Routes",
                column: "BusServicesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Services_BusServicesId",
                table: "Routes",
                column: "BusServicesId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
