namespace LightInvest.Models.Simulacao.Tarifa
{
	public class SelecionarTarifaViewModel
	{
		public string TarifaEscolhida { get; set; }
		public decimal PrecoKwh { get; set; }

		public List<string> TiposDeTarifa { get; set; } = Enum.GetNames(typeof(TipoTarifa)).ToList();
	}


}
