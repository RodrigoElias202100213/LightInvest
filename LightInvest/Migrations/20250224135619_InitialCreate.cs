using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROICalculators");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
