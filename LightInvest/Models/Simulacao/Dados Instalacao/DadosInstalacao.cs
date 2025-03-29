using LightInvest.Models.b;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.Ener
{
	/// <summary>
	/// Represents the difficulty levels for the installation process of solar panels.
	/// </summary>
	public enum DificuldadeInstalacao
	{
		/// <summary>
		/// Represents an easy installation process.
		/// </summary>
		Facil,

		/// <summary>
		/// Represents a medium-level difficulty installation process.
		/// </summary>
		Media,

		/// <summary>
		/// Represents a difficult installation process.
		/// </summary>
		Dificil
	}

	/// <summary>
	/// Represents the installation data for a solar panel system, including location, panel model, and installation parameters.
	/// </summary>
	public class DadosInstalacao
	{
		/// <summary>
		/// Gets or sets the unique identifier for the installation data.
		/// </summary>
		/// <value>
		/// An integer representing the unique identifier for the installation data.
		/// </value>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the email address of the user requesting the installation.
		/// </summary>
		/// <value>
		/// A string containing the user's email address.
		/// </value>
		[Required]
		[EmailAddress]
		public string UserEmail { get; set; }

		/// <summary>
		/// Gets or sets the city identifier where the installation will take place.
		/// </summary>
		/// <value>
		/// An integer representing the foreign key to the city.
		/// </value>
		[Required]
		public int CidadeId { get; set; }

		/// <summary>
		/// Gets or sets the city associated with the installation.
		/// </summary>
		/// <value>
		/// A <see cref="Cidade"/> object representing the city where the installation will occur.
		/// </value>
		[ForeignKey(nameof(CidadeId))]
		public virtual Cidade Cidade { get; set; }

		/// <summary>
		/// Gets or sets the panel model identifier for the installation.
		/// </summary>
		/// <value>
		/// An integer representing the foreign key to the solar panel model.
		/// </value>
		[Required]
		public int ModeloPainelId { get; set; }

		/// <summary>
		/// Gets or sets the solar panel model associated with the installation.
		/// </summary>
		/// <value>
		/// A <see cref="ModeloPainelSolar"/> object representing the panel model selected.
		/// </value>
		[ForeignKey(nameof(ModeloPainelId))]
		public virtual ModeloPainelSolar ModeloPainel { get; set; }

		/// <summary>
		/// Gets or sets the panel power rating identifier.
		/// </summary>
		/// <value>
		/// An integer representing the foreign key to the solar panel power rating.
		/// </value>
		[Required]
		public int PotenciaId { get; set; }

		/// <summary>
		/// Gets or sets the power rating associated with the solar panel.
		/// </summary>
		/// <value>
		/// A <see cref="PotenciaPainelSolar"/> object representing the power rating of the panel.
		/// </value>
		[ForeignKey(nameof(PotenciaId))]
		public virtual PotenciaPainelSolar Potencia { get; set; }

		/// <summary>
		/// Gets or sets the number of panels to be installed.
		/// </summary>
		/// <value>
		/// An integer representing the number of panels to be installed.
		/// </value>
		[Range(1, 1000)]
		public int NumeroPaineis { get; set; }

		/// <summary>
		/// Gets or sets the tilt angle for the panels.
		/// </summary>
		/// <value>
		/// A decimal representing the tilt angle of the panels in degrees (0 to 90).
		/// </value>
		[Range(0, 90)]
		public decimal Inclinacao { get; set; }

		/// <summary>
		/// Gets or sets the difficulty level for the installation.
		/// </summary>
		/// <value>
		/// A <see cref="DificuldadeInstalacao"/> value representing the difficulty of the installation.
		/// </value>
		[Required]
		public DificuldadeInstalacao? Dificuldade { get; set; } = null;

		/// <summary>
		/// Gets or sets the total price of the installation.
		/// </summary>
		/// <value>
		/// A decimal value representing the total price for the installation.
		/// </value>
		public decimal PrecoInstalacao { get; set; }

		/// <summary>
		/// Updates the installation price by recalculating it based on the current parameters.
		/// </summary>
		public void AtualizarPrecoInstalacao()
		{
			PrecoInstalacao = CalcularPrecoInstalacao();
		}

		/// <summary>
		/// Calculates the price adjustment factor based on the tilt angle of the panels.
		/// </summary>
		/// <returns>
		/// A decimal value representing the price adjustment factor for the tilt angle.
		/// </returns>
		public decimal CalcularPrecoPorInclinacao()
		{
			if (Inclinacao <= 35)
				return 0.3m;
			if (Inclinacao <= 60)
				return 0.5m;
			return 0.7m;
		}

		/// <summary>
		/// Calculates the price adjustment factor based on the difficulty of the installation.
		/// </summary>
		/// <returns>
		/// A decimal value representing the price adjustment factor for the installation difficulty.
		/// </returns>
		public decimal CalcularPrecoPorDificuldade()
		{
			return Dificuldade switch
			{
				DificuldadeInstalacao.Facil => 1.0m,
				DificuldadeInstalacao.Media => 1.2m,
				DificuldadeInstalacao.Dificil => 1.5m,
				_ => 1.0m
			};
		}

		/// <summary>
		/// Calculates the total installation price based on the selected panel model, power rating, tilt angle, difficulty, and number of panels.
		/// </summary>
		/// <returns>
		/// A decimal value representing the total installation price.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Thrown if the panel model is not loaded correctly.
		/// </exception>
		public decimal CalcularPrecoInstalacao()
		{
			if (ModeloPainel == null)
			{
				throw new InvalidOperationException("O modelo do painel não foi carregado corretamente.");
			}

			decimal precoBase = ModeloPainel.Preco;
			decimal fatorInclinacao = CalcularPrecoPorInclinacao();
			decimal fatorDificuldade = CalcularPrecoPorDificuldade();

			return precoBase * fatorInclinacao * fatorDificuldade * NumeroPaineis;
		}
	}
}
