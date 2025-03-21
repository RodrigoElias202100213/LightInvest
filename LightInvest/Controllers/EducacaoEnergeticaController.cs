using Microsoft.AspNetCore.Mvc;
using LightInvest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LightInvest.Controllers
{
    public class EducacaoEnergeticaController : Controller
    {
        // Lista simulada de artigos educativos
        private readonly List<EducacaoEnergetica> _artigos = new List<EducacaoEnergetica>
        {
            new EducacaoEnergetica
            {
                Id = 1,
                Titulo = "Benefícios das Energias Renováveis",
                Categoria = "Energia Renovável",
                Resumo = "Descubra os benefícios econômicos e ambientais da adoção de sistemas renováveis.",
                Conteudo = "Conteúdo completo sobre os benefícios das energias renováveis...",
                DataPublicacao = DateTime.Now.AddDays(-15),
                Autor = "Autor 1"
            },
            new EducacaoEnergetica
            {
                Id = 2,
                Titulo = "Princípios Básicos de Painéis Solares",
                Categoria = "Painéis Solares",
                Resumo = "Aprenda os fundamentos do funcionamento dos painéis solares.",
                Conteudo = "Conteúdo completo sobre os princípios básicos dos painéis solares...",
                DataPublicacao = DateTime.Now.AddDays(-10),
                Autor = "Autor 2"
            },
            new EducacaoEnergetica
            {
                Id = 3,
                Titulo = "Cálculo do ROI e Economia Total",
                Categoria = "Cálculos do ROI",
                Resumo = "Entenda como calcular o Retorno sobre Investimento (ROI) em sistemas de energia renovável.",
                Conteudo = "Conteúdo completo sobre o cálculo do ROI e economia total...",
                DataPublicacao = DateTime.Now.AddDays(-5),
                Autor = "Autor 3"
            }
        };

        // GET: /EducacaoEnergetica/Index
        public IActionResult Index()
        {
            // Retorna a lista de artigos para a view
            return View(_artigos);
        }

        // GET: /EducacaoEnergetica/Details/{id}
        public IActionResult Details(int id)
        {
            var artigo = _artigos.FirstOrDefault(a => a.Id == id);
            if (artigo == null)
            {
                return NotFound();
            }
            return View(artigo);
        }
    }
}
