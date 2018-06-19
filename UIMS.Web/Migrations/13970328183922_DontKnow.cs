using Microsoft.EntityFrameworkCore.Migrations;

namespace UIMS.Web.Migrations
{
    public partial class DontKnow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "BuildingClass",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enable",
                table: "BuildingClass");
        }
    }
}
