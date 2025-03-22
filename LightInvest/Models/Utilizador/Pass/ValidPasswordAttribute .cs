using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LightInvest.Models.Utilizador.Pass
{
	public class ValidPasswordAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value == null)
				return false;

			var password = value.ToString();

			if (password.Length < 8)
				return false;

			if (password.Count(char.IsDigit) < 2)
				return false;

			if (!password.Any(char.IsUpper))
				return false;

			return true;
		}

		public override string FormatErrorMessage(string name)
		{
			return "A palavra-passe deve ter pelo menos 8 caracteres, 2 números e 1 letra maiúscula.";
		}
	}
}