namespace LightInvest.Models
{
	public class SimulacaoCompletaViewModel
	{
		public EnergyConsumptionViewModel EnergyConsumptionViewModel { get; set; }
		public TarifaViewModel TarifaViewModel { get; set; }
		public ResultadoTarifaViewModel ResultadoTarifaViewModel { get; set; }
		public DadosInstalacao DadosInstalacao { get; set; }
		public RoiCalculatorDashboardViewModel ROI { get; set; } // ROI da instalação

		// Nova propriedade: evolução do investimento agrupada por ano
		public List<RetornoInvestimentoAno> RetornoInvestimentoPorAno { get; set; }

		public SimulacaoCompletaViewModel()
		{
			RetornoInvestimentoPorAno = new List<RetornoInvestimentoAno>(); // Inicializa lista vazia
		}
	}
}