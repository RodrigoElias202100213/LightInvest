using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightInvest.Migrations
{
	/// <inheritdoc />
	public partial class UpdateRoiCalculatorWithEmail : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Remover a coluna UserId e criar a relação por Email
			migrationBuilder.DropForeignKey(
				name: "FK_ROICalculators_Users_UserId",
				table: "ROICalculators");

			migrationBuilder.DropIndex(
				name: "IX_ROICalculators_UserId",
				table: "ROICalculators");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "ROICalculators");

			// Adiciona a coluna Email na ROICalculators
			migrationBuilder.AddColumn<string>(
				name: "Email",
				table: "ROICalculators",
				type: "nvarchar(max)",
				nullable: false);

			migrationBuilder.CreateIndex(
				name: "IX_ROICalculators_Email",
				table: "ROICalculators",
				column: "Email");

			// Alterar a chave estrangeira para Email
			migrationBuilder.AddForeignKey(
				name: "FK_ROICalculators_Users_Email",
				table: "ROICalculators",
				column: "Email",
				principalTable: "Users",
				principalColumn: "Email",
				onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// Caso precise reverter, remova a coluna Email e volte para UserId
			migrationBuilder.DropForeignKey(
				name: "FK_ROICalculators_Users_Email",
				table: "ROICalculators");

			migrationBuilder.DropIndex(
				name: "IX_ROICalculators_Email",
				table: "ROICalculators");

			migrationBuilder.DropColumn(
				name: "Email",
				table: "ROICalculators");

			migrationBuilder.AddColumn<int>(
				name: "UserId",
				table: "ROICalculators",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.CreateIndex(
				name: "IX_ROICalculators_UserId",
				table: "ROICalculators",
				column: "UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_ROICalculators_Users_UserId",
				table: "ROICalculators",
				column: "UserId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}

}
