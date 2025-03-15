using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class SeedCidadesAndPaineis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cidades",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Lisboa" },
                    { 2, "Porto" },
                    { 3, "Coimbra" },
                    { 4, "Funchal" }
                });

            migrationBuilder.InsertData(
                table: "ModelosDePaineisSolares",
                columns: new[] { "Id", "Modelo" },
                values: new object[,]
                {
                    { 1, "Modelo A" },
                    { 2, "Modelo B" },
                    { 3, "Modelo C" }
                });

            migrationBuilder.InsertData(
                table: "PotenciasDePaineisSolares",
                columns: new[] { "Id", "PainelSolarId", "Potencia" },
                values: new object[,]
                {
                    { 1, 1, 250m },
                    { 2, 2, 300m },
                    { 3, 3, 350m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
