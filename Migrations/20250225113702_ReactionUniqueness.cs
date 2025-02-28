using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleSocialApp.Migrations
{
    /// <inheritdoc />
    public partial class ReactionUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId_PostId",
                table: "Reactions",
                columns: new[] { "UserId", "PostId" },
                unique: true,
                filter: "[PostId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reactions_UserId_PostId",
                table: "Reactions");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_UserId",
                table: "Reactions",
                column: "UserId");
        }
    }
}
