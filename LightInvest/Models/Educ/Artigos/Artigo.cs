using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models.Educ.Artigos
{
    /// <summary>
    /// Represents an article with content, category, related articles, and comments.
    /// </summary>
    public class Artigo
    {
        /// <summary>
        /// Unique identifier for the article.
        /// </summary>
        [Key]
        public int ArtigoId { get; set; }

        /// <summary>
        /// Title of the article.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }

        /// <summary>
        /// Main content of the article.
        /// </summary>
        [Required]
        public string Conteudo { get; set; }

        /// <summary>
        /// URL of the article's image.
        /// </summary>
        public string ImagemUrl { get; set; }

        /// <summary>
        /// Category to which the article belongs.
        /// </summary>
        public string Categoria { get; set; }

        /// <summary>
        /// Short description of the article.
        /// </summary>
        [MaxLength(500)]
        public string DescricaoCurta { get; set; }

        /// <summary>
        /// Date when the article was published.
        /// </summary>
        public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// List of related articles.
        /// </summary>
        public List<Artigo> ArtigosRelacionados { get; set; }

        /// <summary>
        /// List of comments associated with the article.
        /// </summary>
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}