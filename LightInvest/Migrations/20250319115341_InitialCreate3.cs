using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 1,
                column: "Preco",
                value: 1250.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2,
                column: "Preco",
                value: 1320.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3,
                column: "Preco",
                value: 1280.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 4,
                column: "Preco",
                value: 1300.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 5,
                column: "Preco",
                value: 1230.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 6,
                column: "Preco",
                value: 1270.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 7,
                column: "Preco",
                value: 1260.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 8,
                column: "Preco",
                value: 1240.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 9,
                column: "Preco",
                value: 1290.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 10,
                column: "Preco",
                value: 910.00m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 1,
                column: "Preco",
                value: 250.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 2,
                column: "Preco",
                value: 320.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 3,
                column: "Preco",
                value: 280.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 4,
                column: "Preco",
                value: 300.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 5,
                column: "Preco",
                value: 230.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 6,
                column: "Preco",
                value: 270.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 7,
                column: "Preco",
                value: 260.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 8,
                column: "Preco",
                value: 240.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 9,
                column: "Preco",
                value: 290.00m);

            migrationBuilder.UpdateData(
                table: "ModelosDePaineisSolares",
                keyColumn: "Id",
                keyValue: 10,
                column: "Preco",
                value: 210.00m);
        }
    }
}
