using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleSocialApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMediaIdFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_User1Id",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships");

            migrationBuilder.AlterColumn<string>(
                name: "MediaId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_User1Id",
                table: "Friendships",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_User1Id",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships");

            migrationBuilder.AlterColumn<string>(
                name: "MediaId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_User1Id",
                table: "Friendships",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_User2Id",
                table: "Friendships",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
