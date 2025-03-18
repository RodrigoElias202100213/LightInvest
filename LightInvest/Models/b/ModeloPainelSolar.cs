using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.b
{
	public class ModeloPainelSolar
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Modelo { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Preco { get; set; }

		[InverseProperty("PainelSolar")]
		public List<PotenciaPainelSolar> Potencias { get; set; }
	}
}