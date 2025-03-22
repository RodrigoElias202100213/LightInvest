using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoEnergyConsumption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ConsumoFimSemana",
                table: "EnergyConsumptions",
                type: "nvarchar(max)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)",
                oldMaxLength: 24);

            migrationBuilder.AlterColumn<string>(
                name: "ConsumoDiaSemana",
                table: "EnergyConsumptions",
                type: "nvarchar(max)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)",
                oldMaxLength: 24);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ConsumoFimSemana",
                table: "EnergyConsumptions",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 24);

            migrationBuilder.AlterColumn<string>(
                name: "ConsumoDiaSemana",
                table: "EnergyConsumptions",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 24);
        }
    }
}
