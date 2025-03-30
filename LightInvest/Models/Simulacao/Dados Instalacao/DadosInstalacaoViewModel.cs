using LightInvest.Models.b;
using LightInvest.Models.Ener;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
	/// <summary>
	/// Represents the view model for installation data, including the city, panel model, power rating, number of panels, tilt angle, and installation difficulty.
	/// </summary>
	public class DadosInstalacaoViewModel
	{
		/// <summary>
		/// Gets or sets the city identifier for the installation.
		/// </summary>
		/// <value>
		/// An integer representing the city identifier.
		/// </value>
		[Required(ErrorMessage = "A cidade é obrigatória.")]
		public int CidadeId { get; set; }

		/// <summary>
		/// Gets or sets the panel model identifier for the installation.
		/// </summary>
		/// <value>
		/// An integer representing the panel model identifier.
		/// </value>
		[Required(ErrorMessage = "O modelo do painel solar é obrigatório.")]
		public int ModeloPainelId { get; set; }

		/// <summary>
		/// Gets or sets the panel power rating identifier.
		/// </summary>
		/// <value>
		/// An integer representing the panel power rating identifier.
		/// </value>
		[Required(ErrorMessage = "A potência do painel solar é obrigatória.")]
		public int PotenciaId { get; set; }

		/// <summary>
		/// Gets or sets the number of panels to be installed.
		/// </summary>
		/// <value>
		/// An integer representing the number of panels to be installed, ranging from 1 to 1000.
		/// </value>
		[Required]
		[Range(1, 1000, ErrorMessage = "O número de painéis deve estar entre 1 e 1000.")]
		public int NumeroPaineis { get; set; }

		/// <summary>
		/// Gets or sets the tilt angle for the panels.
		/// </summary>
		/// <value>
		/// A decimal representing the tilt angle of the panels in degrees (0 to 90).
		/// </value>
		[Required]
		[Range(0, 90, ErrorMessage = "A inclinação deve estar entre 0 e 90 graus.")]
		public decimal Inclinacao { get; set; }

		/// <summary>
		/// Gets or sets the installation difficulty.
		/// </summary>
		/// <value>
		/// A <see cref="DificuldadeInstalacao"/> value representing the difficulty of the installation.
		/// </value>
		[Required(ErrorMessage = "A dificuldade de instalação é obrigatória.")]
		public DificuldadeInstalacao? Dificuldade { get; set; } = null;

		/// <summary>
		/// Gets or sets the total installation price.
		/// </summary>
		/// <value>
		/// A decimal representing the total price of the installation.
		/// </value>
		public decimal PrecoInstalacao { get; set; }

		/// <summary>
		/// Gets or sets the list of cities available for installation.
		/// </summary>
		/// <value>
		/// A list of <see cref="Cidade"/> objects representing the available cities for installation.
		/// </value>
		public List<Cidade> Cidades { get; set; } = new();

		/// <summary>
		/// Gets or sets the list of available panel models.
		/// </summary>
		/// <value>
		/// A list of <see cref="ModeloPainelSolar"/> objects representing the available solar panel models.
		/// </value>
		public List<ModeloPainelSolar> ModelosDePaineis { get; set; } = new();

		/// <summary>
		/// Gets or sets the list of available panel power ratings.
		/// </summary>
		/// <value>
		/// A list of <see cref="PotenciaPainelSolar"/> objects representing the available power ratings for the panels.
		/// </value>
		public List<PotenciaPainelSolar> Potencias { get; set; } = new();
	}
}
