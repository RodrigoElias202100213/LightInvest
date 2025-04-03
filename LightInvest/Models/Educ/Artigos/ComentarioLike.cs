using LightInvest.Models.Utilizador.Login;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LightInvest.Models.Educ.Artigos
{
	/// <summary>
	/// Represents a like or dislike on a comment made by a user.
	/// </summary>
	public class ComentarioLike
	{
		/// <summary>
		/// Gets or sets the unique identifier for the like or dislike.
		/// </summary>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the comment that the like or dislike is associated with.
		/// This property is a foreign key referencing the Comentario table.
		/// </summary>
		public int ComentarioId { get; set; }

		/// <summary>
		/// Navigation property for the comment related to this like or dislike.
		/// </summary>
		[ForeignKey("ComentarioId")]
		public Comentario Comentario { get; set; }

		/// <summary>
		/// Gets or sets the identifier of the user who made the like or dislike.
		/// This property is a foreign key referencing the User table.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Navigation property for the user who made the like or dislike.
		/// </summary>
		[ForeignKey("UserId")]
		public User User { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the reaction is a like or dislike.
		/// If true, the reaction is a like. If false, the reaction is a dislike.
		/// </summary>
		public bool IsLike { get; set; }
	}
}
