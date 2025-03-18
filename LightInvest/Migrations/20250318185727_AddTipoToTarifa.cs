using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoToTarifa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrecoKwh",
                table: "Tarifas",
                newName: "PrecoKWh");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Tarifas",
                newName: "Tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrecoKWh",
                table: "Tarifas",
                newName: "PrecoKwh");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Tarifas",
                newName: "Nome");
        }
    }
}
