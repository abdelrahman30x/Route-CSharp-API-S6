using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceG02.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adresses_ApplicationUserId",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "Adresses");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "Adresses");

            migrationBuilder.CreateIndex(
                name: "IX_Adresses_ApplicationUserId",
                table: "Adresses",
                column: "ApplicationUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adresses_ApplicationUserId",
                table: "Adresses");

            migrationBuilder.AddColumn<int>(
                name: "AddressType",
                table: "Adresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "Adresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Adresses_ApplicationUserId",
                table: "Adresses",
                column: "ApplicationUserId");
        }
    }
}
