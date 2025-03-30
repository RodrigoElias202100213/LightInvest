using LightInvest.Models.Utilizador.Login;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.Educ.Artigos
{
	public class Comentario
	{
		/// <summary>
		/// Gets or sets the unique identifier for the comment.
		/// </summary>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the content of the comment.
		/// </summary>
		[Required]
		[MaxLength(500)]
		public string Texto { get; set; }

		/// <summary>
		/// Gets or sets the author of the comment.
		/// </summary>
		[Required]
		[MaxLength(100)]
		public string Autor { get; set; } = "nome";

		/// <summary>
		/// Gets or sets the date when the comment was created.
		/// </summary>
		public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets the article to which the comment belongs.
		/// </summary>
		public int ArtigoId { get; set; }

		/// <summary>
		/// Navigation property for the article related to this comment.
		/// </summary>
		[ForeignKey("ArtigoId")]
		public Artigo Artigo { get; set; }

		/// <summary>
		/// Gets or sets the user who made the comment.
		/// </summary>
		public int? UserId { get; set; }

		/// <summary>
		/// Navigation property for the user who made the comment.
		/// </summary>
		[ForeignKey("UserId")]
		public User User { get; set; }
	}
}
