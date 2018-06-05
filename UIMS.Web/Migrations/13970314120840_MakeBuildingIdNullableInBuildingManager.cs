using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UIMS.Web.Migrations
{
    public partial class MakeBuildingIdNullableInBuildingManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuildingManager_Building_BuildingId",
                table: "BuildingManager");

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "BuildingManager",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingManager_Building_BuildingId",
                table: "BuildingManager",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuildingManager_Building_BuildingId",
                table: "BuildingManager");

            migrationBuilder.AlterColumn<int>(
                name: "BuildingId",
                table: "BuildingManager",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingManager_Building_BuildingId",
                table: "BuildingManager",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
