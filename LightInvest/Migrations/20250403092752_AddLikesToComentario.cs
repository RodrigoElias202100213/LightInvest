using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightInvest.Migrations
{
    /// <inheritdoc />
    public partial class AddLikesToComentario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 1,
                column: "DescricaoCurta",
                value: "Entenda os benefícios da energia solar para a sua residência ou empresa.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Artigos",
                keyColumn: "ArtigoId",
                keyValue: 1,
                column: "DescricaoCurta",
                value: "Entenda os benefícios da energia solar para sua residência ou empresa.");
        }
    }
}
