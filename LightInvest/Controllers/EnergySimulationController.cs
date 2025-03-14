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

		return View(model);
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
		decimal fatorDescontoHorario = 0.9m, fatorDescontoMes = 1.0m, consumoTotal = 0;

		model.MediaSemana = Math.Round(model.ConsumoDiaSemana.Average(), 2);
		model.MediaFimSemana = Math.Round(model.ConsumoFimSemana.Average(), 2);

		foreach (var mes in model.MesesOcupacao)
		{
			fatorDescontoMes = mesesComPrecoMaisBarato.Contains(mes) ? 0.9m : 1.0m;
			for (int hora = 0; hora < 24; hora++)
			{
				decimal fatorDescontoHora = horariosTarifaReduzida.Any(h => hora >= h.inicio && hora <= h.fim) ? fatorDescontoHorario : 1.0m;
				consumoTotal += ((model.ConsumoDiaSemana[hora] + model.ConsumoFimSemana[hora]) / 2) * fatorDescontoHora * fatorDescontoMes;
			}
		}
		model.MediaAnual = model.MesesOcupacao.Any() ? Math.Round(consumoTotal * (model.MesesOcupacao.Count / 12.0m), 2) : 0;
		return consumoTotal;
	}

	private async Task SaveConsumptionToDatabase(string userEmail, EnergyConsumptionViewModel model, decimal consumoTotal)
	{
		// Verifique se já existe um registro de consumo para o usuário
		var consumoExistente = await _context.EnergyConsumptions
			.FirstOrDefaultAsync(c => c.UserEmail == userEmail);

		if (consumoExistente != null)
		{
			// Atualize os dados do consumo existente
			consumoExistente.ConsumoDiaSemana = model.ConsumoDiaSemana;
			consumoExistente.ConsumoFimSemana = model.ConsumoFimSemana;
			consumoExistente.MesesOcupacao = model.MesesOcupacao;
			consumoExistente.MediaSemana = model.MediaSemana;
			consumoExistente.MediaFimSemana = model.MediaFimSemana;
			consumoExistente.MediaAnual = model.MediaAnual;
			consumoExistente.ConsumoTotal = consumoTotal;

			// Sinalize que a entidade foi modificada
			_context.Entry(consumoExistente).State = EntityState.Modified;
		}
		else
		{
			// Crie um novo registro caso não exista um anterior
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

			_context.EnergyConsumptions.Add(consumo); // Adiciona um novo registro
		}

		await _context.SaveChangesAsync(); // Salva as alterações no banco
	}

	private void StoreTempData(EnergyConsumptionViewModel model)
	{
		TempData["ConsumoTotal"] = model.MediaAnual.ToString("F2");
		TempData["MesesOcupacao"] = model.MesesOcupacao;
	}


	[HttpPost]
	public IActionResult SimularCusto(ResultadoTarifaViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View("Index", model);
		}

		// Lógica para calcular o custo da tarifa
		return View("ResultadoSimulacao", model);


		// Definir a tarifa escolhida
		if (Enum.TryParse(model.TarifaEscolhida, out TipoTarifa tipoTarifa))
		{
			var tarifa = new Tarifa { Nome = tipoTarifa, PrecoKwh = TarifasBase[tipoTarifa] };
			model.PrecoKwh = tarifa.GetPrecoKwh(); // Aplica acréscimos se necessário
		}

		// Simular o consumo mensal com base na ocupação dos meses
		model.ConsumoMensal = CalcularConsumoMensal(model);

		// Calcular o consumo total anual e o custo anual
		model.ConsumoTotal = model.ConsumoMensal.Sum(m => m.Consumo);
		model.ValorAnual = model.ConsumoMensal.Sum(m => m.Custo);

		// Retornar os dados atualizados para a view
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

}

	