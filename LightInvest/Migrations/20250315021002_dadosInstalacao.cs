using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class dadosInstalacao : Migration
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
                    Dificuldade = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                name: "PotenciasDePaineisSolares");

            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "ModelosDePaineisSolares");
        }
    }
}
