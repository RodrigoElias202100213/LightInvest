using System;
using System.Collections.Generic;
using System.Linq;

namespace LightInvest.Models
{
	public class TarifaViewModel
	{
		public TipoTarifa TipoDeTarifaEscolhida { get; set; }
		public decimal PrecoKWh { get; set; }
		public List<string> TiposDeTarifa { get; set; }

		public TarifaViewModel()
		{
			TiposDeTarifa = Enum.GetNames(typeof(TipoTarifa)).ToList();
		}

		public TarifaViewModel(Tarifa tarifa) : this()
		{
			TipoDeTarifaEscolhida = tarifa.Tipo;
			PrecoKWh = tarifa.PrecoKWh;
		}
	}

	public class ResultadoTarifaViewModel
	{
		public decimal ConsumoTotal { get; set; }
		public List<MesConsumo> ConsumoMensal { get; set; }
		public decimal ValorAnual { get; set; }
		public string TarifaEscolhida { get; set; }
		public decimal PrecoKwh { get; set; }
		public List<string> MesesOcupacao { get; set; }

		public ResultadoTarifaViewModel()
		{
			MesesOcupacao = new List<string>();
			ConsumoMensal = new List<MesConsumo>();
		}

		public void AtualizarPrecoKwh(TipoTarifa tipoTarifa, decimal precoBase)
		{
			var tarifa = new Tarifa { Tipo = tipoTarifa, PrecoKWh = precoBase };
			PrecoKwh = tarifa.PrecoKWh;
		}
	}

	public class MesConsumo
	{
		public string Mes { get; set; }
		public decimal Consumo { get; set; }
		public decimal Custo { get; set; }
	}

	public enum Meses
	{
		Janeiro, Fevereiro, Marco, Abril, Maio, Junho,
		Julho, Agosto, Setembro, Outubro, Novembro, Dezembro
	}
}