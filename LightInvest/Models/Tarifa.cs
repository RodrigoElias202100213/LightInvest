using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LightInvest.Models
{
	public enum TipoTarifa
	{
		Residencial,
		Comercial,
		Industrial
	}

	public class Tarifa
	{
		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "O preço por kWh deve ser maior que zero.")]
		public decimal PrecoKWh { get; set; }

<<<<<<< HEAD
			[Key] public int Id { get; set; }

			[Required][EmailAddress] public string UserEmail { get; set; }

			public DateTime DataAlteracao { get; private set; } = DateTime.Now;
=======
		[Key] public int Id { get; set; }

		[Required][EmailAddress] public string UserEmail { get; set; }

		public DateTime DataAlteracao { get; private set; } = DateTime.Now;
>>>>>>> backup



		[Required(ErrorMessage = "Por favor, selecione o tipo de tarifa.")]
		public TipoTarifa Tipo { get; set; }

<<<<<<< HEAD
			public decimal PrecoFinal => PrecoKWh + ObterValorExtra();

			private decimal ObterValorExtra() => Tipo switch
			{
				TipoTarifa.Residencial => 0.1m,
				TipoTarifa.Comercial => 0.5m,
				TipoTarifa.Industrial => 0.9m,
				_ => throw new ArgumentException("Tipo de tarifa inválido")
			};
		}
	}

=======
		public decimal PrecoFinal => PrecoKWh + ObterValorExtra();

		private decimal ObterValorExtra() => Tipo switch
		{
			TipoTarifa.Residencial => 0.1m,
			TipoTarifa.Comercial => 0.5m,
			TipoTarifa.Industrial => 0.9m,
			_ => throw new ArgumentException("Tipo de tarifa inválido")
		};
	}
}
>>>>>>> backup












