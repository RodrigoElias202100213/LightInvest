using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class AddComentarioLikeTable4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsAdmin", "Name", "Password" },
                values: new object[,]
                {
                    { 2, "veragfernandes04@gmail.com", true, "Vera", "Vera1234" },
                    { 3, "ligthinvestuser@gmail.com", false, "LightInvest ", "LightInvest123" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
