using LightInvest.Models.Ener;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.b
{
	/// <summary>
	/// Represents a city in the system, with a name and associated installation data.
	/// </summary>
	public class Cidade
	{
		/// <summary>
		/// Gets or sets the unique identifier for the city.
		/// </summary>
		/// <value>
		/// An integer representing the unique identifier of the city.
		/// </value>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the city.
		/// </summary>
		/// <value>
		/// A string representing the name of the city. The maximum length is 100 characters.
		/// </value>
		[Required]
		[StringLength(100)]
		public string Nome { get; set; }

		/// <summary>
		/// Gets or sets the collection of installation data associated with the city.
		/// </summary>
		/// <value>
		/// A collection of <see cref="DadosInstalacao"/> instances representing installation data related to the city.
		/// </value>
		public virtual ICollection<DadosInstalacao> DadosInstalacoes { get; set; } = new List<DadosInstalacao>();
	}
}
