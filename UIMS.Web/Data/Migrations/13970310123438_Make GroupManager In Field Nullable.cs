using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace UIMS.Web.Data.Migrations
{
    public partial class MakeGroupManagerInFieldNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_GroupManager_GroupManagerId",
                table: "Field");

            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "Semester",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "Presentation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "GroupManagerId",
                table: "Field",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Field_GroupManager_GroupManagerId",
                table: "Field",
                column: "GroupManagerId",
                principalTable: "GroupManager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_GroupManager_GroupManagerId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "Enable",
                table: "Semester");

            migrationBuilder.DropColumn(
                name: "Enable",
                table: "Presentation");

            migrationBuilder.AlterColumn<int>(
                name: "GroupManagerId",
                table: "Field",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_GroupManager_GroupManagerId",
                table: "Field",
                column: "GroupManagerId",
                principalTable: "GroupManager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
