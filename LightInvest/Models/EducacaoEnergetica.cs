using System;
using System.ComponentModel.DataAnnotations;

namespace LightInvest.Models
{
    public class EducacaoEnergetica
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título deve ter até 200 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }
        // Exemplos de categorias: "Energia Renovável", "Painéis Solares", "Cálculos do ROI", "Eficiência Energética"

        [Required(ErrorMessage = "O resumo é obrigatório.")]
        [Display(Name = "Resumo")]
        public string Resumo { get; set; }
        // Breve descrição do conteúdo para exibição na listagem de artigos

        [Required(ErrorMessage = "O conteúdo do artigo é obrigatório.")]
        public string Conteudo { get; set; }
        // Texto completo do artigo, podendo incluir formatação e imagens (em HTML ou outro formato)

        [Display(Name = "Data de Publicação")]
        public DateTime DataPublicacao { get; set; } = DateTime.Now;

        [Display(Name = "Autor")]
        public string Autor { get; set; }
    }
}
