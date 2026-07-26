using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fluxify.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mig_ChatRestore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Chat",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Chat",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Chat",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_CreatedUserId",
                table: "Chat",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_Users_CreatedUserId",
                table: "Chat",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_Users_CreatedUserId",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_CreatedUserId",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Chat");
        }
    }
}
