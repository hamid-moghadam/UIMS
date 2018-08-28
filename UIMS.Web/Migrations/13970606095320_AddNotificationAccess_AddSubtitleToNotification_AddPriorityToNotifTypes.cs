using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UIMS.Web.Migrations
{
    public partial class AddNotificationAccess_AddSubtitleToNotification_AddPriorityToNotifTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "NotificationType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "Notification",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationAccess",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NotificationTypeId = table.Column<int>(nullable: false),
                    AppRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationAccess_AspNetRoles_AppRoleId",
                        column: x => x.AppRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationAccess_NotificationType_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationAccess_AppRoleId",
                table: "NotificationAccess",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationAccess_NotificationTypeId",
                table: "NotificationAccess",
                column: "NotificationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationAccess");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "NotificationType");

            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "Notification");
        }
    }
}
