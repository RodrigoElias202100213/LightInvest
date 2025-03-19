namespace LightInvest.Models
{
	public class SimulacaoCompletaViewModel
	{
		public EnergyConsumptionViewModel EnergyConsumptionViewModel { get; set; }
		public TarifaViewModel TarifaViewModel { get; set; }
		public ResultadoTarifaViewModel ResultadoTarifaViewModel { get; set; }
		public DadosInstalacao DadosInstalacao { get; set; }

		public RoiCalculatorDashboardViewModel ROI { get; set; } // ROI da instalação

		public List<RetornoInvestimentoMes>
			RetornoInvestimento { get; set; } // Novo: Lista com a evolução do investimento

		public SimulacaoCompletaViewModel()
		{
			RetornoInvestimento = new List<RetornoInvestimentoMes>(); // Inicializa lista vazia
		}
	}
}