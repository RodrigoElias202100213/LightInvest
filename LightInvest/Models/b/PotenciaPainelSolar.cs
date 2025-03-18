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

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Potencia { get; set; }

		public ModeloPainelSolar PainelSolar { get; set; }
	}
}