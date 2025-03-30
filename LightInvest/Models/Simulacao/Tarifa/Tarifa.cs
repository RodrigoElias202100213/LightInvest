using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LightInvest.Models.Simulacao.Tarifa
{
	/// <summary>
	/// Defines the available types of energy tariffs.
	/// </summary>
	public enum TipoTarifa
	{
		/// <summary>
		/// Represents the residential tariff type.
		/// </summary>
		Residencial,

		/// <summary>
		/// Represents the commercial tariff type.
		/// </summary>
		Comercial,

		/// <summary>
		/// Represents the industrial tariff type.
		/// </summary>
		Industrial
	}

	/// <summary>
	/// Represents an energy tariff with a price per kilowatt-hour (kWh) and associated tariff type.
	/// </summary>
	public class Tarifa
	{
		/// <summary>
		/// Gets or sets the price per kilowatt-hour (kWh) for the tariff.
		/// </summary>
		/// <value>
		/// A decimal representing the price per kWh.
		/// </value>
		/// <exception cref="ArgumentException">Thrown when the price is less than or equal to zero.</exception>
		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "O preço por kWh deve ser maior que zero.")]
		public decimal PrecoKWh { get; set; }

		/// <summary>
		/// Gets or sets the unique identifier for the tariff.
		/// </summary>
		/// <value>
		/// The tariff's identifier.
		/// </value>
		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the email of the user associated with the tariff.
		/// </summary>
		/// <value>
		/// The email address of the user.
		/// </value>
		[Required]
		[EmailAddress]
		public string UserEmail { get; set; }

		/// <summary>
		/// Gets the date when the tariff was last modified.
		/// </summary>
		/// <value>
		/// A <see cref="DateTime"/> indicating when the tariff was last modified.
		/// </value>
		public DateTime DataAlteracao { get; private set; } = DateTime.Now;

		/// <summary>
		/// Gets or sets the type of the tariff (e.g., Residential, Commercial, Industrial).
		/// </summary>
		/// <value>
		/// A <see cref="TipoTarifa"/> representing the type of the tariff.
		/// </value>
		[Required(ErrorMessage = "Por favor, selecione o tipo de tarifa.")]
		public TipoTarifa Tipo { get; set; }

		/// <summary>
		/// Gets the final price for the tariff, including any extra values based on the tariff type.
		/// </summary>
		/// <value>
		/// A decimal representing the final price per kWh, including extra charges.
		/// </value>
		public decimal PrecoFinal => PrecoKWh + ObterValorExtra();

		/// <summary>
		/// Calculates the extra value to be added to the base price based on the tariff type.
		/// </summary>
		/// <returns>
		/// A decimal representing the additional value based on the tariff type.
		/// </returns>
		private decimal ObterValorExtra() => Tipo switch
		{
			TipoTarifa.Residencial => 0.1m,
			TipoTarifa.Comercial => 0.5m,
			TipoTarifa.Industrial => 0.9m,
			_ => throw new ArgumentException("Tipo de tarifa inválido")
		};
	}
}