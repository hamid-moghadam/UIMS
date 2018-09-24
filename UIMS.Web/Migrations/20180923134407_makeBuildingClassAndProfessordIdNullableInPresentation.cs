using Microsoft.EntityFrameworkCore.Migrations;

namespace UIMS.Web.Migrations
{
    public partial class makeBuildingClassAndProfessordIdNullableInPresentation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentation_BuildingClass_BuildingClassId",
                table: "Presentation");

            migrationBuilder.DropForeignKey(
                name: "FK_Presentation_Professor_ProfessorId",
                table: "Presentation");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessorId",
                table: "Presentation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BuildingClassId",
                table: "Presentation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Presentation_BuildingClass_BuildingClassId",
                table: "Presentation",
                column: "BuildingClassId",
                principalTable: "BuildingClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Presentation_Professor_ProfessorId",
                table: "Presentation",
                column: "ProfessorId",
                principalTable: "Professor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentation_BuildingClass_BuildingClassId",
                table: "Presentation");

            migrationBuilder.DropForeignKey(
                name: "FK_Presentation_Professor_ProfessorId",
                table: "Presentation");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessorId",
                table: "Presentation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuildingClassId",
                table: "Presentation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Presentation_BuildingClass_BuildingClassId",
                table: "Presentation",
                column: "BuildingClassId",
                principalTable: "BuildingClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Presentation_Professor_ProfessorId",
                table: "Presentation",
                column: "ProfessorId",
                principalTable: "Professor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
