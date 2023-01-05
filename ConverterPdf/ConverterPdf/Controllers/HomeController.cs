using ConverterPdf.Models;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace ConverterPdf.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public async Task<byte[]> Index()
		{
			string html = "<td> Test</td>";
			var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "ApplicationName");
			Directory.CreateDirectory(directory);
			var filePath = Path.Combine(directory, $"{Guid.NewGuid()}.html");
			await System.IO.File.WriteAllTextAsync(filePath, html);
			

			var driverOptions = new ChromeOptions();
			// In headless mode, PDF writing is enabled by default (tested with driver major version 85)
			driverOptions.AddArgument("headless");
			using var driver = new ChromeDriver(driverOptions);
			driver.Navigate().GoToUrl(filePath);

			// Output a PDF of the first page in A4 size at 90% scale
			var printOptions = new Dictionary<string, object>
	{
		{ "paperWidth", 210 / 25.4 },
		{ "paperHeight", 297 / 25.4 },
		{ "scale", 0.9 },
		{ "pageRanges", "1" }
	};
			var printOutput = driver.ExecuteCustomDriverCommand("Page.printToPDF", printOptions) as Dictionary<string, object>;
			var pdf = Convert.FromBase64String(printOutput["data"] as string);

			//File.Delete(filePath);

			return pdf;
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}