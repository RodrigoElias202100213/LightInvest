namespace LightInvest.Models
{
	public class SimulacaoCompletaViewModel
	{
		public EnergyConsumptionViewModel EnergyConsumptionViewModel { get; set; }
		public ResultadoTarifaViewModel ResultadoTarifaViewModel { get; set; }
		public DadosInstalacao DadosInstalacao { get; set; }
		public decimal PrecoInstalacao { get; set; }
		public decimal PotenciaPainel { get; set; }
		public RoiCalculatorDashboardViewModel ROI { get; set; }

	}

}
