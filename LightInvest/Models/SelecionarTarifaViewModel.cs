
namespace LightInvest.Models
{
	public class SelecionarTarifaViewModel
	{
		public string TarifaEscolhida { get; set; }
		public decimal PrecoKwh { get; set; }

		// Populate this list from the controller or initialize with enum values
		public List<string> TiposDeTarifa { get; set; } = Enum.GetNames(typeof(TipoTarifa)).ToList();
	}


}
