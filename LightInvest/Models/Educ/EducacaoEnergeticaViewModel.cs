using LightInvest.Models.Educ.Artigos;
using System.Collections.Generic;

namespace LightInvest.Models.Educ
{
    /// <summary>
    /// ViewModel for Energy Education, containing a list of articles.
    /// </summary>
    public class EducacaoEnergeticaViewModel
    {
        /// <summary>
        /// List of articles related to energy education.
        /// </summary>
        public List<Artigo> Artigos { get; set; } = new List<Artigo>();
    }
}