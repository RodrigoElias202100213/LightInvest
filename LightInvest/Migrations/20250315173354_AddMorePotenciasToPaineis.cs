using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePotenciasToPaineis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 1, 270m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 1, 300m });

            migrationBuilder.InsertData(
                table: "PotenciasDePaineisSolares",
                columns: new[] { "Id", "PainelSolarId", "Potencia" },
                values: new object[,]
                {
                    { 4, 2, 300m },
                    { 5, 2, 320m },
                    { 6, 2, 350m },
                    { 7, 3, 350m },
                    { 8, 3, 380m },
                    { 9, 3, 400m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 300m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 3, 350m });
        }
    }
}
