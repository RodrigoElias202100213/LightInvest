using System.Diagnostics;
using System.IO;

namespace LightInvest.Models
{
	public class PdfGenerator
	{
		private readonly string _wkhtmltopdfPath;

		public PdfGenerator(string wkhtmltopdfPath)
		{
			_wkhtmltopdfPath = wkhtmltopdfPath;
		}

		public byte[] GeneratePdf(string htmlContent)
		{
			var tempHtmlFile = Path.Combine(Path.GetTempPath(), "temp.html");
			var tempPdfFile = Path.Combine(Path.GetTempPath(), "output.pdf");

			// Salvar o conteúdo HTML em um arquivo temporário
			File.WriteAllText(tempHtmlFile, htmlContent);

			// Executar o wkhtmltopdf para gerar o PDF
			var startInfo = new ProcessStartInfo
			{
				FileName = _wkhtmltopdfPath,
				Arguments = $"\"{tempHtmlFile}\" \"{tempPdfFile}\"",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			};

			using (var process = Process.Start(startInfo))
			{
				process.WaitForExit();
			}

			// Ler o PDF gerado e retornar como byte array
			var pdfBytes = File.ReadAllBytes(tempPdfFile);

			// Excluir os arquivos temporários
			File.Delete(tempHtmlFile);
			File.Delete(tempPdfFile);

			return pdfBytes;
		}
	}
}