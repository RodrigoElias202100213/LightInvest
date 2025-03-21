namespace LightInvest.Models
{
	public class SimulacaoCompletaViewModel
	{
		public EnergyConsumptionViewModel EnergyConsumptionViewModel { get; set; }
		public TarifaViewModel TarifaViewModel { get; set; }
		public ResultadoTarifaViewModel ResultadoTarifaViewModel { get; set; }
		public DadosInstalacao DadosInstalacao { get; set; }
<<<<<<< HEAD
		public RoiCalculatorDashboardViewModel ROI { get; set; } 
=======
		public RoiCalculatorDashboardViewModel ROI { get; set; }
>>>>>>> backup

		public List<RetornoInvestimentoAno> RetornoInvestimentoPorAno { get; set; }

		public SimulacaoCompletaViewModel()
		{
<<<<<<< HEAD
			RetornoInvestimentoPorAno = new List<RetornoInvestimentoAno>(); 
=======
			RetornoInvestimentoPorAno = new List<RetornoInvestimentoAno>();
>>>>>>> backup
		}
	}
}