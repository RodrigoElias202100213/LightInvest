using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.b
{
	/// <summary>
	/// Represents a model of a solar panel, including its name, price, and available power ratings.
	/// </summary>
	public class ModeloPainelSolar
	{
		/// <summary>
		/// Gets or sets the unique identifier for the solar panel model.
		/// </summary>
		/// <value>
		/// An integer representing the unique identifier for the solar panel model.
		/// </value>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the solar panel model.
		/// </summary>
		/// <value>
		/// A string representing the name of the solar panel model. The maximum length is 100 characters.
		/// </value>
		[Required]
		[StringLength(100)]
		public string ModeloNome { get; set; }

		/// <summary>
		/// Gets or sets the price of the solar panel model.
		/// </summary>
		/// <value>
		/// A decimal value representing the price of the solar panel model, with two decimal places.
		/// </value>
		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Preco { get; set; }

		/// <summary>
		/// Gets or sets the list of power ratings associated with the solar panel model.
		/// </summary>
		/// <value>
		/// A collection of <see cref="PotenciaPainelSolar"/> instances representing the power ratings of the solar panel.
		/// </value>
		public List<PotenciaPainelSolar> Potencias { get; set; }
	}
}
