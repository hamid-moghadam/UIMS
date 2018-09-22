using Microsoft.EntityFrameworkCore.Migrations;

namespace UIMS.Web.Migrations
{
    public partial class makeDayInPresentationNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Day",
                table: "Presentation",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Day",
                table: "Presentation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
