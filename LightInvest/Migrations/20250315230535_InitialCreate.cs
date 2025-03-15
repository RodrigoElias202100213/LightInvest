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
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nome = table.Column<int>(type: "int", nullable: false),
                    PrecoKwh = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                name: "DadosInstalacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CidadeId = table.Column<int>(type: "int", nullable: false),
                    ModeloPainelId = table.Column<int>(type: "int", nullable: false),
                    NumeroPaineis = table.Column<int>(type: "int", nullable: false),
                    ConsumoPainel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Inclinacao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dificuldade = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "PotenciasDePaineisSolares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PainelSolarId = table.Column<int>(type: "int", nullable: false),
                    Potencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PotenciasDePaineisSolares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PotenciasDePaineisSolares_ModelosDePaineisSolares_PainelSolarId",
                        column: x => x.PainelSolarId,
                        principalTable: "ModelosDePaineisSolares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    { 2, 1, 270m },
                    { 3, 1, 300m },
                    { 4, 2, 300m },
                    { 5, 2, 320m },
                    { 6, 2, 350m },
                    { 7, 3, 350m },
                    { 8, 3, 380m },
                    { 9, 3, 400m }
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
                name: "IX_PotenciasDePaineisSolares_PainelSolarId",
                table: "PotenciasDePaineisSolares",
                column: "PainelSolarId");
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
                name: "PotenciasDePaineisSolares");

            migrationBuilder.DropTable(
                name: "ROICalculators");

            migrationBuilder.DropTable(
                name: "Tarifas");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "ModelosDePaineisSolares");
        }
    }
}
