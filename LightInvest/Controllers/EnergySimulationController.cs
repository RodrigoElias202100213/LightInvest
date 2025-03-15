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
	public async Task<IActionResult> SimularCusto(ResultadoTarifaViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View("Index", model);
		}

		// Exemplo de como chamar a função de salvar a tarifa
		if (Enum.TryParse(model.TarifaEscolhida, out TipoTarifa tipoTarifa))
		{
			var tarifa = new Tarifa { Nome = tipoTarifa, PrecoKwh = TarifasBase[tipoTarifa] };
			model.PrecoKwh = tarifa.GetPrecoKwh(); // Aplica acréscimos se necessário

			// Salvar no banco de dados
			await SaveTarifaToDatabase(tarifa);  // Passando o objeto Tarifa
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

	public IActionResult Tarifas()
	{
		return View();
	}

	[HttpPost("save-tarifa")]
	public async Task<IActionResult> SaveTarifa(string tarifaEscolhida)
	{
		if (Enum.TryParse(tarifaEscolhida, out TipoTarifa tipoTarifa))
		{
			// Recupera o preço do kWh com base no tipo de tarifa
			decimal precoKwh = TarifasBase[tipoTarifa];

			// Cria uma nova instância de Tarifa
			var tarifa = new Tarifa
			{
				Nome = tipoTarifa,
				PrecoKwh = precoKwh
			};

			// Adiciona a nova tarifa ao contexto e salva no banco
			_context.Tarifas.Add(tarifa);
			await _context.SaveChangesAsync();

			// Redireciona ou exibe mensagem de sucesso
			ViewBag.Resultado = "Tarifa salva com sucesso!";
			return RedirectToAction("Tarifas");
		}

		// Caso o tipo de tarifa não seja válido
		ViewBag.Resultado = "Erro: Tipo de tarifa inválido!";
		return View("Index");
	}

	[HttpGet]
	public IActionResult SelecionarTarifa()
	{
		var model = new SelecionarTarifaViewModel
		{
			// Fill TiposDeTarifa with the enum values or any predefined list of tariffs
			TiposDeTarifa = Enum.GetNames(typeof(TipoTarifa)).ToList()
		};

		return View(model);
	}

	[HttpPost]
	public async Task<IActionResult> SelecionarTarifa(SelecionarTarifaViewModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);  // Se o modelo não for válido, retorne a view com o modelo atual
		}

		var user = await GetLoggedInUserAsync();
		if (user == null)
		{
			ViewBag.Resultado = "Erro: Nenhum utilizador autenticado!";
			return View("Index", model);
		}

		// Tenta converter o tipo de tarifa escolhido pelo usuário para o enum TipoTarifa
		if (Enum.TryParse(model.TarifaEscolhida, out TipoTarifa tipoTarifa))
		{
			// Recupera o preço da tarifa com base no tipo selecionado
			var tarifaBase = TarifasBase[tipoTarifa];

			// O valor do Preço por kWh será a soma do valor da tarifa base com o valor inserido pelo usuário
			model.PrecoKwh = tarifaBase + model.PrecoKwh;

			// Cria uma nova instância de Tarifa com os dados do usuário e o preço calculado
			var tarifa = new Tarifa
			{
				Nome = tipoTarifa,
				PrecoKwh = model.PrecoKwh,
				UserEmail = user.Email
			};

			// Salva a tarifa no banco de dados
			await SaveTarifaToDatabase(tarifa);

			// Redireciona o utilizador para a página de resultados da simulação
			return RedirectToAction("ResultadoSimulacao", model);
		}

		// Caso a conversão do tipo de tarifa falhe, retorna uma mensagem de erro
		ViewBag.Resultado = "Erro: Tipo de tarifa inválido!";
		return View(model);  // Retorna a view com a mensagem de erro
	}

	public IActionResult ResultadoSimulacao(ResultadoTarifaViewModel model)
	{
		// Aqui você pode processar o model e retornar a View
		return View(model); // Retorna a view de resultado
	}

	private async Task SaveTarifaToDatabase(Tarifa tarifa)
	{
		// Verifica se já existe uma tarifa para o mesmo usuário (por email)
		var tarifaExistente = await _context.Tarifas
			.Where(t => t.UserEmail == tarifa.UserEmail)  // Busca pela tarifa do usuário com base no email
			.FirstOrDefaultAsync();

		if (tarifaExistente != null)
		{
			// Se a tarifa já existir, atualiza os valores de PrecoKwh e DataAlteracao
			tarifaExistente.PrecoKwh = tarifa.PrecoKwh;  // Atualiza o preço da tarifa
			tarifaExistente.DataAlteracao = DateTime.Now;  // Atualiza a data de alteração

			// Atualiza a tarifa no banco de dados
			_context.Tarifas.Update(tarifaExistente);
		}
		else
		{
			// Caso não exista, cria a tarifa com a data atual
			tarifa.DataAlteracao = DateTime.Now;  // Define a data de alteração
			_context.Tarifas.Add(tarifa);  // Adiciona a tarifa inicial
		}

		// Salva as alterações no banco de dados
		await _context.SaveChangesAsync();
	}

}

