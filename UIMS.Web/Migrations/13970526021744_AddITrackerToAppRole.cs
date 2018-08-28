using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UIMS.Web.Migrations
{
    public partial class AddITrackerToAppRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: DateTime.Now);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "AspNetRoles");
        }
    }
}
