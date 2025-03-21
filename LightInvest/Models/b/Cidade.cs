using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.b
{
	public class Cidade
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Nome { get; set; }

<<<<<<< HEAD
		// Navegação para os dados de instalação relacionados
=======
>>>>>>> backup
		public virtual ICollection<DadosInstalacao> DadosInstalacoes { get; set; } = new List<DadosInstalacao>();
	}
}