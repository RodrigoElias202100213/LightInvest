using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.b
{
	public class PotenciaPainelSolar
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("PainelSolar")]
		public int PainelSolarId { get; set; }

		public decimal Potencia { get; set; } // Potência do painel solar

		public ModeloPainelSolar PainelSolar { get; set; } // Relacionamento com ModeloPainelSolar
	}
}