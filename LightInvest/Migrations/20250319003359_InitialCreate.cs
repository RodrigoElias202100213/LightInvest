using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnergyConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumoDiaSemana = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsumoFimSemana = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MesesOcupacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaSemana = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MediaFimSemana = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MediaAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyConsumptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelosDePaineisSolares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModeloNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelosDePaineisSolares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ROICalculators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustoInstalacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustoManutencaoAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumoEnergeticoMedio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumoEnergeticoRede = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetornoEconomia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ROI = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCalculado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROICalculators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarifas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrecoKWh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PotenciasDePaineisSolares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Potencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModeloPainelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotenciasDePaineisSolares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PotenciasDePaineisSolares_ModelosDePaineisSolares_ModeloPainelId",
                        column: x => x.ModeloPainelId,
                        principalTable: "ModelosDePaineisSolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DadosInstalacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CidadeId = table.Column<int>(type: "int", nullable: false),
                    ModeloPainelId = table.Column<int>(type: "int", nullable: false),
                    PotenciaId = table.Column<int>(type: "int", nullable: false),
                    NumeroPaineis = table.Column<int>(type: "int", nullable: false),
                    Inclinacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dificuldade = table.Column<int>(type: "int", nullable: false),
                    PrecoInstalacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosInstalacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosInstalacao_Cidades_CidadeId",
                        column: x => x.CidadeId,
                        principalTable: "Cidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DadosInstalacao_ModelosDePaineisSolares_ModeloPainelId",
                        column: x => x.ModeloPainelId,
                        principalTable: "ModelosDePaineisSolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DadosInstalacao_PotenciasDePaineisSolares_PotenciaId",
                        column: x => x.PotenciaId,
                        principalTable: "PotenciasDePaineisSolares",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Cidades",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Albufeira" },
                    { 2, "Almada" },
                    { 3, "Amadora" },
                    { 4, "Aveiro" },
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

            migrationBuilder.InsertData(
                table: "ModelosDePaineisSolares",
                columns: new[] { "Id", "ModeloNome", "Preco" },
                values: new object[,]
                {
                    { 1, "Aiko - Comet 2U", 0m },
                    { 2, "Maxeon 7", 0m },
                    { 3, "Longi - HI-MO X6", 0m },
                    { 4, "Huasun - Himalaya", 0m },
                    { 5, "TW Solar", 0m },
                    { 6, "JA Solar DeepBlue 4.0 Pro", 0m },
                    { 7, "Astroenergy - Astro N5", 0m },
                    { 8, "Grand Sunergy", 0m },
                    { 9, "DMEGC - Infinity RT", 0m },
                    { 10, "Spic", 0m }
                });

            migrationBuilder.InsertData(
                table: "PotenciasDePaineisSolares",
                columns: new[] { "Id", "ModeloPainelId", "Potencia" },
                values: new object[,]
                {
                    { 1, 1, 670m },
                    { 2, 1, 680m },
                    { 3, 1, 690m },
                    { 4, 1, 700m },
                    { 5, 2, 445m },
                    { 6, 2, 455m },
                    { 7, 2, 465m },
                    { 8, 2, 475m },
                    { 9, 3, 600m },
                    { 10, 3, 610m },
                    { 11, 3, 620m },
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

            migrationBuilder.CreateIndex(
                name: "IX_DadosInstalacao_CidadeId",
                table: "DadosInstalacao",
                column: "CidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_DadosInstalacao_ModeloPainelId",
                table: "DadosInstalacao",
                column: "ModeloPainelId");

            migrationBuilder.CreateIndex(
                name: "IX_DadosInstalacao_PotenciaId",
                table: "DadosInstalacao",
                column: "PotenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_PotenciasDePaineisSolares_ModeloPainelId",
                table: "PotenciasDePaineisSolares",
                column: "ModeloPainelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosInstalacao");

            migrationBuilder.DropTable(
                name: "EnergyConsumptions");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "ROICalculators");

            migrationBuilder.DropTable(
                name: "Tarifas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "PotenciasDePaineisSolares");

            migrationBuilder.DropTable(
                name: "ModelosDePaineisSolares");
        }
    }
}
