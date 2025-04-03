using LightInvest.Models.Utilizador.Login;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.Educ.Artigos
{
	public class ComentarioLike
	{
		[Key]
		public int Id { get; set; }

		public int ComentarioId { get; set; }

		[ForeignKey("ComentarioId")]
		public Comentario Comentario { get; set; }

		public int UserId { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		// Esta propriedade vai indicar se o like é positivo (true) ou negativo (false).
		public bool IsLike { get; set; }
	}
}
