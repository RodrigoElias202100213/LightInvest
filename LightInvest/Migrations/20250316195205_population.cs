using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class population : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nome",
                value: "Albufeira");

            migrationBuilder.UpdateData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nome",
                value: "Almada");

            migrationBuilder.UpdateData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nome",
                value: "Amadora");

            migrationBuilder.UpdateData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 4,
                column: "Nome",
                value: "Aveiro");

            migrationBuilder.InsertData(
                table: "Cidades",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 5, "Barcelos" },
                    { 6, "Beja" },
                    { 7, "Braga" },
                    { 8, "Bragança" },
                    { 9, "Caldas da Rainha" },
                    { 10, "Cascais" },
                    { 11, "Coimbra" },
                    { 12, "Évora" },
                    { 13, "Faro" },
                    { 14, "Figueira da Foz" },
                    { 15, "Funchal" },
                    { 16, "Guarda" },
                    { 17, "Guimarães" },
                    { 18, "Leiria" },
                    { 19, "Lisboa" },
                    { 20, "Matosinhos" },
                    { 21, "Montijo" },
                    { 22, "Odivelas" },
                    { 23, "Oeiras" },
                    { 24, "Portalegre" },
                    { 25, "Portimão" },
                    { 26, "Porto" },
                    { 27, "Póvoa de Varzim" },
                    { 28, "Santarem" },
                    { 29, "Setúbal" },
                    { 30, "Barreiro" },
                    { 31, "Sintra" },
                    { 32, "Tomar" },
                    { 33, "Torres Vedras" },
                    { 34, "Viana do castelo" },
                    { 35, "Vila do Conde" },
                    { 36, "Vila Nova de Gaia" },
                    { 37, "Viseu" }
                });

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 1,
                column: "Modelo",
                value: "Aiko - Comet 2U");

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2,
                column: "Modelo",
                value: "Maxeon 7");

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3,
                column: "Modelo",
                value: "Longi - HI-MO X6");

            migrationBuilder.InsertData(
                table: "ModelosDePaineisSolares",
                columns: new[] { "Id", "Modelo" },
                values: new object[,]
                {
                    { 4, "Huasun - Himalaya" },
                    { 5, "TW Solar" },
                    { 6, "JA Solar DeepBlue 4.0 Pro" },
                    { 7, "Astroenergy - Astro N5" },
                    { 8, "Grand Sunergy" },
                    { 9, "DMEGC - Infinity RT" },
                    { 10, "Spic" }
                });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 1,
                column: "Potencia",
                value: 670m);

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

            migrationBuilder.InsertData(
                table: "PotenciasDePaineisSolares",
                columns: new[] { "Id", "PainelSolarId", "Potencia" },
                values: new object[,]
                {
                    { 10, 9, 615m },
                    { 11, 10, 410m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nome",
                value: "Lisboa");

            migrationBuilder.UpdateData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nome",
                value: "Porto");

            migrationBuilder.UpdateData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nome",
                value: "Coimbra");

            migrationBuilder.UpdateData(
                table: "Cidades",
                keyColumn: "Id",
                keyValue: 4,
                column: "Nome",
                value: "Funchal");

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 1,
                column: "Modelo",
                value: "Modelo A");

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2,
                column: "Modelo",
                value: "Modelo B");

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3,
                column: "Modelo",
                value: "Modelo C");

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 1,
                column: "Potencia",
                value: 250m);

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

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 300m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 320m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 2, 350m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 3, 350m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 3, 380m });

            migrationBuilder.UpdateData(
                table: "PotenciasDePaineisSolares",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "PainelSolarId", "Potencia" },
                values: new object[] { 3, 400m });
        }
    }
}
