using LightInvest.Models.Utilizador.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.Educ.Artigos
{
	/// <summary>
	/// Represents a comment made on an article in the system.
	/// </summary>
	public class Comentario
	{
		/// <summary>
		/// Gets or sets the unique identifier for the comment.
		/// </summary>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the content of the comment.
		/// The content must not exceed 500 characters.
		/// </summary>
		[Required]
		[MaxLength(500)]
		public string Texto { get; set; }

		/// <summary>
		/// Gets or sets the author of the comment.
		/// The author's name is a string with a maximum length of 100 characters.
		/// Default value is "nome" if no author is specified.
		/// </summary>
		[Required]
		[MaxLength(100)]
		public string Autor { get; set; } = "nome";

		/// <summary>
		/// Gets or sets the date when the comment was created.
		/// This value is automatically set to the current UTC time when the comment is created.
		/// </summary>
		public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Gets or sets the article to which the comment belongs.
		/// The ArticleId is a foreign key pointing to the related article.
		/// </summary>
		public int ArtigoId { get; set; }

		/// <summary>
		/// Navigation property for the article related to this comment.
		/// </summary>
		[ForeignKey("ArtigoId")]
		public Artigo Artigo { get; set; }

		/// <summary>
		/// Gets or sets the user who made the comment.
		/// The UserId is an optional foreign key that references the user who made the comment.
		/// </summary>
		public int? UserId { get; set; }

		/// <summary>
		/// Navigation property for the user who made the comment.
		/// This property will be null if the comment is anonymous (i.e., the user is not logged in).
		/// </summary>
		[ForeignKey("UserId")]
		public User User { get; set; }

		/// <summary>
		/// Navigation property for the Likes (and Dislikes) associated with the comment.
		/// A list of ComentarioLike entities representing users' reactions to the comment.
		/// </summary>
		public List<ComentarioLike> Likes { get; set; } = new List<ComentarioLike>();

		/// <summary>
		/// A property that is not mapped to the database.
		/// This property is used to track whether the current user has liked the comment.
		/// It is used for UI logic to show whether the comment has been liked by the user.
		/// </summary>
		[NotMapped]
		public bool liked { get; set; }

		/// <summary>
		/// A property that is not mapped to the database.
		/// This property is used to track whether the current user has disliked the comment.
		/// It is used for UI logic to show whether the comment has been disliked by the user.
		/// </summary>
		[NotMapped]
		public bool disliked { get; set; }
	}
}
