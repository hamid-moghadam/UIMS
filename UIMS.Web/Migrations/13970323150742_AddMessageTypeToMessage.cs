using Microsoft.EntityFrameworkCore.Migrations;

namespace UIMS.Web.Migrations
{
    public partial class AddMessageTypeToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_MessageType_MessageTypeId",
                table: "Message");

            migrationBuilder.AlterColumn<int>(
                name: "MessageTypeId",
                table: "Message",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_MessageType_MessageTypeId",
                table: "Message",
                column: "MessageTypeId",
                principalTable: "MessageType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_MessageType_MessageTypeId",
                table: "Message");

            migrationBuilder.AlterColumn<int>(
                name: "MessageTypeId",
                table: "Message",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Message_MessageType_MessageTypeId",
                table: "Message",
                column: "MessageTypeId",
                principalTable: "MessageType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
