using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.b
{
	/// <summary>
	/// Represents the power rating of a specific solar panel model.
	/// </summary>
	public class PotenciaPainelSolar
	{
		/// <summary>
		/// Gets or sets the unique identifier for the power rating of the solar panel.
		/// </summary>
		/// <value>
		/// An integer representing the unique identifier for the power rating.
		/// </value>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the power rating of the solar panel in kilowatts (kW).
		/// </summary>
		/// <value>
		/// A decimal value representing the power rating of the solar panel.
		/// </value>
		public decimal Potencia { get; set; }

		/// <summary>
		/// Gets or sets the identifier for the associated solar panel model.
		/// </summary>
		/// <value>
		/// An integer representing the foreign key to the solar panel model.
		/// </value>
		[ForeignKey("ModeloPainelSolar")]
		public int ModeloPainelId { get; set; }

		/// <summary>
		/// Gets or sets the associated solar panel model for this power rating.
		/// </summary>
		/// <value>
		/// A <see cref="ModeloPainelSolar"/> instance representing the solar panel model.
		/// </value>
		public ModeloPainelSolar ModeloPainelSolar { get; set; }
	}
}
