using System;
using System.Linq;
using System.Threading.Tasks;
using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightInvest.Controllers
{
	public class TarifaController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TarifaController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Exibe o formulário de simulação/definição da tarifa
		[HttpGet("tarifa-simulation")]
		public async Task<IActionResult> Simulation()
		{
			var model = InitializeViewModel();
			LoadTempData(model);
			return View(model);
		}

		private TarifaViewModel InitializeViewModel()
		{
			// Inicializa o ViewModel com os valores padrão;
			// a lista TiposDeTarifa já é populada no construtor do TarifaViewModel.
			return new TarifaViewModel();
		}

		private void LoadTempData(TarifaViewModel model)
		{
			// Se houver informação temporária (como o PrecoFinal calculado anteriormente) no TempData, podemos usá-la para exibir uma mensagem.
			if (TempData["PrecoFinal"] != null)
			{
				// Exemplo: você pode passar essa informação via ViewBag ou incluir uma propriedade no view model para exibição.
				ViewBag.PrecoFinal = TempData["PrecoFinal"].ToString();
			}
		}

		// Recupera o usuário logado a partir do e-mail armazenado na sessão
		private async Task<User> GetLoggedInUserAsync()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			return string.IsNullOrEmpty(userEmail)
				? null
				: await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
		}

		// Processa o formulário de simulação/definição de tarifa
		[HttpPost("tarifa-simulation")]
		public async Task<IActionResult> Simulation(TarifaViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await GetLoggedInUserAsync();
			if (user == null)
			{
				ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
				return View(model);
			}

			// Cria uma instância de Tarifa com os dados do ViewModel e do usuário logado
			var tarifa = new Tarifa
			{
				UserEmail = user.Email,
				PrecoKWh = model.PrecoKWh,
				Tipo = model.TipoDeTarifaEscolhida
				// DataAlteracao é definida automaticamente como DateTime.Now
			};

			// Salva a tarifa no banco de dados (atualizando se já existir ou criando uma nova)
			await SaveTarifaToDatabase(user.Email, tarifa);

			// Armazena o PrecoFinal no TempData para exibição (caso necessário)
			TempData["PrecoFinal"] = tarifa.PrecoFinal.ToString("F2");

			return RedirectToAction("Create", "DadosInstalacao");
		}

		private async Task SaveTarifaToDatabase(string userEmail, Tarifa tarifa)
		{
			// Procura uma tarifa já existente para o usuário logado
			var tarifaExistente = await _context.Tarifas.FirstOrDefaultAsync(t => t.UserEmail == userEmail);

			if (tarifaExistente != null)
			{
				// Atualiza os campos da tarifa já existente
				tarifaExistente.PrecoKWh = tarifa.PrecoKWh;
				tarifaExistente.Tipo = tarifa.Tipo;
				// Atualiza a DataAlteracao para o momento atual (por ser setter privado, usamos reflection)
				typeof(Tarifa).GetProperty("DataAlteracao")?.SetValue(tarifaExistente, DateTime.Now);

				_context.Tarifas.Update(tarifaExistente);
			}
			else
			{
				// Adiciona uma nova tarifa
				_context.Tarifas.Add(tarifa);
			}

			await _context.SaveChangesAsync();
		}
	}
}
