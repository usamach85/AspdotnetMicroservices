using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;

namespace AirLabs.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			var client = new RestClient("http://airlabs.co/api/v9/ping?api_key=YOUR-API-KEY");

			var request = new RestRequest(Method.GET);

			IRestResponse response = client.Execute(request);

		}
	}
}