using Microsoft.EntityFrameworkCore.Migrations;

namespace UIMS.Web.Migrations
{
    public partial class AddPersionNameToAppRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersionName",
                table: "AspNetRoles",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersionName",
                table: "AspNetRoles");
        }
    }
}
