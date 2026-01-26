using Authentication_and_Authorization.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authentication_and_Authorization.Pages
{
    //[Authorize(Policy = "HRManagerOnly")]
    public class HRManagerModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        public List<WeatherForecastDTO>? weatherForecastItems { get; set; }

        public HRManagerModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task OnGetAsync()
        {
            var client = httpClientFactory.CreateClient("TestWebAPI");
            weatherForecastItems = await client.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast");
        }
    }
}
