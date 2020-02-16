using Microsoft.EntityFrameworkCore.Migrations;

namespace Chat.Core.Migrations
{
    public partial class AddChatRoomTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersChatRoom",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<long>(nullable: false),
                    ChatroomId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersChatRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersChatRoom_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersChatRoom_Chatrooms_ChatroomId",
                        column: x => x.ChatroomId,
                        principalTable: "Chatrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersChatRoom_ApplicationUserId",
                table: "UsersChatRoom",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersChatRoom_ChatroomId",
                table: "UsersChatRoom",
                column: "ChatroomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersChatRoom");
        }
    }
}
