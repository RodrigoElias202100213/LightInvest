using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using LightInvest.Data;
using LightInvest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EnergySimulationController : Controller
{
	private readonly ApplicationDbContext _context;
	private static readonly Dictionary<TipoTarifa, decimal> TarifasBase = new()
	{
		{ TipoTarifa.Residencial, 0.50m },
		{ TipoTarifa.Comercial, 0.65m },
		{ TipoTarifa.Industrial, 0.55m }
	};

	public EnergySimulationController(ApplicationDbContext context)
	{
		_context = context;
	}

	[HttpGet("energy-simulation")]
	public async Task<IActionResult> Simulation()
	{
		var model = InitializeViewModel();
		LoadTempData(model);
		return View(model);
	}

	private EnergyConsumptionViewModel InitializeViewModel()
	{
		return new EnergyConsumptionViewModel
		{
			ConsumoDiaSemana = Enumerable.Repeat(0m, 24).ToList(),
			ConsumoFimSemana = Enumerable.Repeat(0m, 24).ToList(),
			MesesOcupacao = new List<string>()
		};
	}

	private void LoadTempData(EnergyConsumptionViewModel model)
	{
		if (TempData["ConsumoTotal"] != null)
		{
			model.MediaAnual = decimal.Parse(TempData["ConsumoTotal"].ToString());
		}
	}

	private async Task<User> GetLoggedInUserAsync()
	{
		var userEmail = HttpContext.Session.GetString("UserEmail");
		return string.IsNullOrEmpty(userEmail) ? null : await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
	}

	[HttpPost("energy-simulation")]
	public async Task<IActionResult> Simulation(EnergyConsumptionViewModel model)
	{
		EnsureValidData(model);

		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return View("Index", model);
		}

		decimal consumoTotal = CalculateTotalConsumption(model);
		await SaveConsumptionToDatabase(user.Email, model, consumoTotal);
		StoreTempData(model);

		// Aqui, retornamos a view com os valores preenchidos, para que o usuário veja os dados novamente
		return View(model); // Aqui passamos o model atualizado com os dados preenchidos.
	}

	private void EnsureValidData(EnergyConsumptionViewModel model)
	{
		model.ConsumoDiaSemana ??= Enumerable.Repeat(0m, 24).ToList();
		model.ConsumoFimSemana ??= Enumerable.Repeat(0m, 24).ToList();
		model.MesesOcupacao ??= new List<string>();
	}


	private decimal CalculateTotalConsumption(EnergyConsumptionViewModel model)
	{
		var mesesComPrecoMaisBarato = new HashSet<string> { "Mar", "Abr", "Mai", "Set", "Out", "Nov" };
		var horariosTarifaReduzida = new List<(int inicio, int fim)> { (0, 6), (9, 12), (22, 23) };
		decimal consumoTotal = 0;

		model.MediaSemana = Math.Round(model.ConsumoDiaSemana.Average(), 2);
		model.MediaFimSemana = Math.Round(model.ConsumoFimSemana.Average(), 2);

		foreach (var mes in model.MesesOcupacao)
		{
			decimal fatorDescontoMes = GetMonthDiscountFactor(mes, mesesComPrecoMaisBarato);
			consumoTotal += CalculateHourlyConsumption(model, horariosTarifaReduzida, fatorDescontoMes);
		}

		model.MediaAnual = model.MesesOcupacao.Any() ? Math.Round(consumoTotal * (model.MesesOcupacao.Count / 12.0m), 2) : 0;
		return consumoTotal;
	}

	private decimal GetMonthDiscountFactor(string mes, HashSet<string> mesesComPrecoMaisBarato)
	{
		return mesesComPrecoMaisBarato.Contains(mes) ? 0.9m : 1.0m;
	}

	private decimal CalculateHourlyConsumption(EnergyConsumptionViewModel model, List<(int inicio, int fim)> horariosTarifaReduzida, decimal fatorDescontoMes)
	{
		decimal consumoTotal = 0;
		for (int hora = 0; hora < 24; hora++)
		{
			decimal fatorDescontoHora = horariosTarifaReduzida.Any(h => hora >= h.inicio && hora <= h.fim) ? 0.9m : 1.0m;
			consumoTotal += ((model.ConsumoDiaSemana[hora] + model.ConsumoFimSemana[hora]) / 2) * fatorDescontoHora * fatorDescontoMes;
		}
		return consumoTotal;
	}

	private async Task SaveConsumptionToDatabase(string userEmail, EnergyConsumptionViewModel model, decimal consumoTotal)
	{
		var consumoExistente = await _context.EnergyConsumptions
			.FirstOrDefaultAsync(c => c.UserEmail == userEmail);

		if (consumoExistente != null)
		{
			consumoExistente.ConsumoDiaSemana = model.ConsumoDiaSemana;
			consumoExistente.ConsumoFimSemana = model.ConsumoFimSemana;
			consumoExistente.MesesOcupacao = model.MesesOcupacao;
			consumoExistente.MediaSemana = model.MediaSemana;
			consumoExistente.MediaFimSemana = model.MediaFimSemana;
			consumoExistente.MediaAnual = model.MediaAnual;
			consumoExistente.ConsumoTotal = consumoTotal;

			_context.Entry(consumoExistente).State = EntityState.Modified;
		}
		else
		{
			var consumo = new EnergyConsumption
			{
				UserEmail = userEmail,
				ConsumoDiaSemana = model.ConsumoDiaSemana,
				ConsumoFimSemana = model.ConsumoFimSemana,
				MesesOcupacao = model.MesesOcupacao,
				MediaSemana = model.MediaSemana,
				MediaFimSemana = model.MediaFimSemana,
				MediaAnual = model.MediaAnual,
				ConsumoTotal = consumoTotal
			};

			_context.EnergyConsumptions.Add(consumo);
		}

		await _context.SaveChangesAsync();
	}

	private void StoreTempData(EnergyConsumptionViewModel model)
	{
		TempData["ConsumoTotal"] = model.MediaAnual.ToString("F2");
		TempData["MesesOcupacao"] = model.MesesOcupacao;
	}

	[HttpPost]
	public async Task<IActionResult> SimularCusto(ResultadoTarifaViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View("Index", model);
		}

		if (!Enum.TryParse(model.TarifaEscolhida, true, out TipoTarifa tipoTarifa))
		{
			ModelState.AddModelError("TarifaEscolhida", "Tipo de tarifa inválido.");
			return View("Index", model);
		}

		var tarifa = new Tarifa { Nome = tipoTarifa, PrecoKwh = TarifasBase[tipoTarifa] };
		model.PrecoKwh = tarifa.GetPrecoKwh();
		await SaveTarifaToDatabase(tarifa);

		model.ConsumoMensal = CalcularConsumoMensal(model);
		model.ConsumoTotal = model.ConsumoMensal.Sum(m => m.Consumo);
		model.ValorAnual = model.ConsumoMensal.Sum(m => m.Custo);

		return View("ResultadoSimulacao", model);
	}


	private List<MesConsumo> CalcularConsumoMensal(ResultadoTarifaViewModel model)
	{
		var mesesDisponiveis = Enum.GetNames(typeof(Meses));
		var consumoMensal = new List<MesConsumo>();

		foreach (var mes in mesesDisponiveis)
		{
			bool estaOcupado = model.MesesOcupacao.Contains(mes);
			decimal consumo = estaOcupado ? new Random().Next(200, 500) : 0; // Simulação de consumo (ajuste conforme necessário)
			decimal custo = consumo * model.PrecoKwh;

			consumoMensal.Add(new MesConsumo { Mes = mes, Consumo = consumo, Custo = custo });
		}

		return consumoMensal;
	}

	public IActionResult Tarifas()
	{
		return View();
	}

	[HttpGet]
	public IActionResult SelecionarTarifa()
	{
		var model = new SelecionarTarifaViewModel
		{
			TiposDeTarifa = Enum.GetNames(typeof(TipoTarifa)).ToList()
		};

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> SelecionarTarifa(SelecionarTarifaViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return View("Index", model);
		}

		if (!Enum.TryParse(model.TarifaEscolhida, true, out TipoTarifa tipoTarifa))
		{
			ModelState.AddModelError("TarifaEscolhida", "Selecione um tipo de tarifa válido.");
			return View(model);
		}

		var tarifaBase = TarifasBase[tipoTarifa];
		model.PrecoKwh = tarifaBase + model.PrecoKwh;

		var tarifa = new Tarifa
		{
			Nome = tipoTarifa,
			PrecoKwh = model.PrecoKwh,
			UserEmail = user.Email
		};

		await SaveTarifaToDatabase(tarifa);
		return RedirectToAction("Create", "DadosInstalacao");
	}

	public IActionResult ResultadoSimulacao(ResultadoTarifaViewModel model)
	{
		return View(model);
	}


	[HttpPost("save-tarifa")]
	public async Task<IActionResult> SaveTarifa(string tarifaEscolhida)
	{
		if (!Enum.TryParse(tarifaEscolhida, true, out TipoTarifa tipoTarifa))
		{
			ViewBag.Resultado = "Erro: Tipo de tarifa inválido!";
			return View("Index");
		}

		decimal precoKwh = TarifasBase[tipoTarifa];

		var tarifa = new Tarifa
		{
			Nome = tipoTarifa, // Salvar o Enum corretamente
			PrecoKwh = precoKwh
		};

		_context.Tarifas.Add(tarifa);
		await _context.SaveChangesAsync();

		ViewBag.Resultado = "Tarifa salva com sucesso!";
		return RedirectToAction("Tarifas");
	}

	private async Task SaveTarifaToDatabase(Tarifa tarifa)
	{
		var tarifaExistente = await _context.Tarifas
			.FirstOrDefaultAsync(t => t.UserEmail == tarifa.UserEmail);

		if (tarifaExistente != null)
		{
			tarifaExistente.PrecoKwh = tarifa.PrecoKwh;
			tarifaExistente.Nome = tarifa.Nome; // Certifique-se de atualizar o nome corretamente
			tarifaExistente.DataAlteracao = DateTime.Now;

			_context.Tarifas.Update(tarifaExistente);
		}
		else
		{
			tarifa.DataAlteracao = DateTime.Now;
			_context.Tarifas.Add(tarifa);
		}

		await _context.SaveChangesAsync();
	}


}