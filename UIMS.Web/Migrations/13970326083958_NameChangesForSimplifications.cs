using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace UIMS.Web.Migrations
{
    public partial class NameChangesForSimplifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_MessageType_MessageTypeId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReceiver_Message_MessageId",
                table: "MessageReceiver");

            migrationBuilder.DropTable(
                name: "ConversationReply");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "MessageReceiver",
                newName: "NotificationId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReceiver_MessageId",
                table: "MessageReceiver",
                newName: "IX_MessageReceiver_NotificationId");

            migrationBuilder.RenameColumn(
                name: "MessageTypeId",
                table: "Message",
                newName: "NotificationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_MessageTypeId",
                table: "Message",
                newName: "IX_Message_NotificationTypeId");

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    FirstUserId = table.Column<int>(nullable: false),
                    SecondUserId = table.Column<int>(nullable: false),
                    Enable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chat_AspNetUsers_FirstUserId",
                        column: x => x.FirstUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chat_AspNetUsers_SecondUserId",
                        column: x => x.SecondUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatReply",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Reply = table.Column<string>(maxLength: 1000, nullable: true),
                    ReplierId = table.Column<int>(nullable: false),
                    ChatId = table.Column<int>(nullable: false),
                    SemesterId = table.Column<int>(nullable: false),
                    HasSeen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatReply_Chat_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatReply_AspNetUsers_ReplierId",
                        column: x => x.ReplierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatReply_Semester_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semester",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chat_FirstUserId",
                table: "Chat",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_SecondUserId",
                table: "Chat",
                column: "SecondUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatReply_ChatId",
                table: "ChatReply",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatReply_ReplierId",
                table: "ChatReply",
                column: "ReplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatReply_SemesterId",
                table: "ChatReply",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_MessageType_NotificationTypeId",
                table: "Message",
                column: "NotificationTypeId",
                principalTable: "MessageType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReceiver_Message_NotificationId",
                table: "MessageReceiver",
                column: "NotificationId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_MessageType_NotificationTypeId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReceiver_Message_NotificationId",
                table: "MessageReceiver");

            migrationBuilder.DropTable(
                name: "ChatReply");

            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.RenameColumn(
                name: "NotificationId",
                table: "MessageReceiver",
                newName: "MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageReceiver_NotificationId",
                table: "MessageReceiver",
                newName: "IX_MessageReceiver_MessageId");

            migrationBuilder.RenameColumn(
                name: "NotificationTypeId",
                table: "Message",
                newName: "MessageTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_NotificationTypeId",
                table: "Message",
                newName: "IX_Message_MessageTypeId");

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Enable = table.Column<bool>(nullable: false),
                    FirstUserId = table.Column<int>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    SecondUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversation_AspNetUsers_FirstUserId",
                        column: x => x.FirstUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversation_AspNetUsers_SecondUserId",
                        column: x => x.SecondUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConversationReply",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ConversationId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    HasSeen = table.Column<bool>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    ReplierId = table.Column<int>(nullable: false),
                    Reply = table.Column<string>(maxLength: 1000, nullable: true),
                    SemesterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationReply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationReply_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationReply_AspNetUsers_ReplierId",
                        column: x => x.ReplierId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationReply_Semester_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semester",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_FirstUserId",
                table: "Conversation",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_SecondUserId",
                table: "Conversation",
                column: "SecondUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationReply_ConversationId",
                table: "ConversationReply",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationReply_ReplierId",
                table: "ConversationReply",
                column: "ReplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationReply_SemesterId",
                table: "ConversationReply",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_MessageType_MessageTypeId",
                table: "Message",
                column: "MessageTypeId",
                principalTable: "MessageType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReceiver_Message_MessageId",
                table: "MessageReceiver",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
