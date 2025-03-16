using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class populationPOWER : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 1, 680m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 1, 690m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 1, 700m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 445m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 455m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 465m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 475m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 3, 600m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 3, 610m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 3, 620m });

            migrationBuilder.InsertData(
                table: "PotenciasDePaineisSolares",
                columns: new[] { "Id", "PainelSolarId", "Potencia" },
                values: new object[,]
                {
                    { 12, 3, 630m },
                    { 13, 4, 720m },
                    { 14, 4, 730m },
                    { 15, 4, 740m },
                    { 16, 4, 750m },
                    { 17, 5, 715m },
                    { 18, 5, 725m },
                    { 19, 5, 735m },
                    { 20, 5, 745m },
                    { 21, 5, 590m },
                    { 22, 5, 600m },
                    { 23, 5, 610m },
                    { 24, 5, 620m },
                    { 25, 6, 595m },
                    { 26, 6, 605m },
                    { 27, 6, 615m },
                    { 28, 6, 625m },
                    { 29, 7, 640m },
                    { 30, 7, 650m },
                    { 31, 7, 660m },
                    { 32, 7, 670m },
                    { 33, 8, 710m },
                    { 34, 8, 720m },
                    { 35, 8, 730m },
                    { 36, 8, 740m },
                    { 37, 9, 615m },
                    { 38, 9, 625m },
                    { 39, 9, 635m },
                    { 40, 9, 645m },
                    { 41, 10, 410m },
                    { 42, 10, 420m },
                    { 43, 10, 430m },
                    { 44, 10, 440m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 445m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 3, 600m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 4, 720m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 5, 715m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 5, 590m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 6, 595m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 7, 640m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 8, 710m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 9, 615m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 10, 410m });
        }
    }
}
