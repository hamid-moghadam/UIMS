using Microsoft.EntityFrameworkCore.Migrations;

namespace UIMS.Web.Migrations
{
    public partial class fixPersianNameInAppRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersionName",
                table: "AspNetRoles",
                newName: "PersianName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersianName",
                table: "AspNetRoles",
                newName: "PersionName");
        }
    }
}
