using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UIMS.Web.Migrations
{
    public partial class AddSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_MessageType_NotificationTypeId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Semester_SemesterId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_AspNetUsers_SenderId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReceiver_Message_NotificationId",
                table: "MessageReceiver");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReceiver_AspNetUsers_UserId",
                table: "MessageReceiver");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageType",
                table: "MessageType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageReceiver",
                table: "MessageReceiver");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.RenameTable(
                name: "MessageType",
                newName: "NotificationType");

            migrationBuilder.RenameTable(
                name: "MessageReceiver",
                newName: "NotificationReceiver");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReceiver_UserId",
                table: "NotificationReceiver",
                newName: "IX_NotificationReceiver_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReceiver_NotificationId",
                table: "NotificationReceiver",
                newName: "IX_NotificationReceiver_NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderId",
                table: "Notification",
                newName: "IX_Notification_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SemesterId",
                table: "Notification",
                newName: "IX_Notification_SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_NotificationTypeId",
                table: "Notification",
                newName: "IX_Notification_NotificationTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationReceiver",
                table: "NotificationReceiver",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    AccessName = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Value = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationType_NotificationTypeId",
                table: "Notification",
                column: "NotificationTypeId",
                principalTable: "NotificationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Semester_SemesterId",
                table: "Notification",
                column: "SemesterId",
                principalTable: "Semester",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_SenderId",
                table: "Notification",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationReceiver_Notification_NotificationId",
                table: "NotificationReceiver",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationReceiver_AspNetUsers_UserId",
                table: "NotificationReceiver",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationType_NotificationTypeId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Semester_SemesterId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_SenderId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationReceiver_Notification_NotificationId",
                table: "NotificationReceiver");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationReceiver_AspNetUsers_UserId",
                table: "NotificationReceiver");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationReceiver",
                table: "NotificationReceiver");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationType",
                newName: "MessageType");

            migrationBuilder.RenameTable(
                name: "NotificationReceiver",
                newName: "MessageReceiver");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Message");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationReceiver_UserId",
                table: "MessageReceiver",
                newName: "IX_MessageReceiver_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationReceiver_NotificationId",
                table: "MessageReceiver",
                newName: "IX_MessageReceiver_NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_SenderId",
                table: "Message",
                newName: "IX_Message_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_SemesterId",
                table: "Message",
                newName: "IX_Message_SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_NotificationTypeId",
                table: "Message",
                newName: "IX_Message_NotificationTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageType",
                table: "MessageType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageReceiver",
                table: "MessageReceiver",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_MessageType_NotificationTypeId",
                table: "Message",
                column: "NotificationTypeId",
                principalTable: "MessageType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Semester_SemesterId",
                table: "Message",
                column: "SemesterId",
                principalTable: "Semester",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_AspNetUsers_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReceiver_Message_NotificationId",
                table: "MessageReceiver",
                column: "NotificationId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReceiver_AspNetUsers_UserId",
                table: "MessageReceiver",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
