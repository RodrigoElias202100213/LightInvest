using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.b
{
	public class PotenciaPainelSolar
	{
		[Key]
		public int Id { get; set; }

		public decimal Potencia { get; set; }
		
		[ForeignKey("ModeloPainelSolar")]
		public int ModeloPainelId { get; set; }

<<<<<<< HEAD
=======
		[ForeignKey("ModeloPainelSolar")]
		public int ModeloPainelId { get; set; }

>>>>>>> backup
		public ModeloPainelSolar ModeloPainelSolar { get; set; }
	}
}
