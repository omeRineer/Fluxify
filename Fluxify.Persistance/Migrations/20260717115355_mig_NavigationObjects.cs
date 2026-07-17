using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fluxify.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_NavigationObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Channels",
                newName: "Slug");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Topics",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_TopicId",
                table: "UserTopics",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_UserId",
                table: "UserTopics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NameSpaceId",
                table: "Users",
                column: "NameSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_NameSpaceId",
                table: "UserDevices",
                column: "NameSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDevices_UserId",
                table: "UserDevices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_ChannelId",
                table: "Topics",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Channels_ChannelId",
                table: "Topics",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_NameSpaces_NameSpaceId",
                table: "UserDevices",
                column: "NameSpaceId",
                principalTable: "NameSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDevices_Users_UserId",
                table: "UserDevices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NameSpaces_NameSpaceId",
                table: "Users",
                column: "NameSpaceId",
                principalTable: "NameSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTopics_Topics_TopicId",
                table: "UserTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTopics_Users_UserId",
                table: "UserTopics",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Channels_ChannelId",
                table: "Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_NameSpaces_NameSpaceId",
                table: "UserDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDevices_Users_UserId",
                table: "UserDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_NameSpaces_NameSpaceId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTopics_Topics_TopicId",
                table: "UserTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTopics_Users_UserId",
                table: "UserTopics");

            migrationBuilder.DropIndex(
                name: "IX_UserTopics_TopicId",
                table: "UserTopics");

            migrationBuilder.DropIndex(
                name: "IX_UserTopics_UserId",
                table: "UserTopics");

            migrationBuilder.DropIndex(
                name: "IX_Users_NameSpaceId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserDevices_NameSpaceId",
                table: "UserDevices");

            migrationBuilder.DropIndex(
                name: "IX_UserDevices_UserId",
                table: "UserDevices");

            migrationBuilder.DropIndex(
                name: "IX_Topics_ChannelId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Topics");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Channels",
                newName: "Key");
        }
    }
}
