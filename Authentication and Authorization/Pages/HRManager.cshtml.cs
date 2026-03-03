using Authentication_and_Authorization.Authorization;
using Authentication_and_Authorization.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Web_API.Models;

namespace Authentication_and_Authorization.Pages
{
    [Authorize(Policy = "HRManagerOnly")]
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
            // tak samo jak client z program.cs
            var client = httpClientFactory.CreateClient("TestWebAPI");
            // DODAJE authentykację do żądania HTTP, aby uzyskać dostęp do chronionego zasobu w WebAPI. Musze zrobic post Credential
            var response = await client.PostAsJsonAsync("Auth", new Credential { UserName = "admin", Password = "admin" });
            // Sprawdza, czy odpowiedź HTTP jest sukcesem (status code 2xx). Jeśli nie, to rzuca wyjątek.
            response.EnsureSuccessStatusCode();
            // Odczytuje zawartość odpowiedzi HTTP jako string, który powinien zawierać token JWT (JSON Web Token) zwrócony przez endpoint "Auth" w WebAPI. 
            string jwt = await response.Content.ReadAsStringAsync();
            // musze go desserializowac do formatu objektowego, aby móc go użyć w nagłówku autoryzacji.
            var token = JsonConvert.DeserializeObject<JwtToken>(jwt);
            // Teraz mając token, chce go wysłać razem z requestem
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken??string.Empty);
            // WeatherForecast to nazwa kontrolera w WebAPI, który zwraca dane pogodowe. Musi być zgodna z nazwą kontrolera w WebAPI (WeatherForecastController).
            weatherForecastItems = await client.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
        }
    }
}
