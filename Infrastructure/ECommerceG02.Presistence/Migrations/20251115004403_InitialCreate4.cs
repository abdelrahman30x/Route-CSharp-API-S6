using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceG02.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adresses_Users_ApplicationUserId",
                table: "Adresses");

            migrationBuilder.DropIndex(
                name: "IX_Adresses_ApplicationUserId",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Adresses");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Adresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adresses_UserId",
                table: "Adresses",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Adresses_Users_UserId",
                table: "Adresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adresses_Users_UserId",
                table: "Adresses");

            migrationBuilder.DropIndex(
                name: "IX_Adresses_UserId",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Adresses");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Adresses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Adresses_ApplicationUserId",
                table: "Adresses",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Adresses_Users_ApplicationUserId",
                table: "Adresses",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
