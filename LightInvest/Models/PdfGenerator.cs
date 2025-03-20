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

			File.WriteAllText(tempHtmlFile, htmlContent);

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

			var pdfBytes = File.ReadAllBytes(tempPdfFile);

			File.Delete(tempHtmlFile);
			File.Delete(tempPdfFile);

			return pdfBytes;
		}
	}
}