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
            // 1. Jesli mamy juz token to nie musze sie za kazdym razem autentykowac, moge go pobrac z Seassion Cookie

            JwtToken token = new JwtToken();
            // nazwa access_token jest dowolna, ale musi być taka sama jak ta, której używamy do przechowywania tokena w sesji.
            var strTokenObj = HttpContext.Session.GetString("access_token");

            // 2. Jesli nie mamy tokena w sesji, to musimy sie autentykowac i pobrac token z WebAPI
            if (string.IsNullOrEmpty(strTokenObj))
            {
                // Authentication and getting a token from WebAPI
                token = await AuthenticateAndGetTokenAsync();
            }
            else
            {
                // Jeśli token jest już dostępny w sesji, to deserializujemy go z formatu string do obiektu JwtToken, aby móc go użyć w dalszej części kodu.
                token = JsonConvert.DeserializeObject<JwtToken>(strTokenObj) ?? new JwtToken();
            }
            if (token == null || token.ExpiresAt <= DateTime.Now || string.IsNullOrWhiteSpace(token.AccessToken))
            {
                // Jeśli token wgasł albo jest z nim jakis inny problem to znów musze go pobrać z WebAPI, czyli muszę się autentykować i pobrać nowy token.
                token = await AuthenticateAndGetTokenAsync();
            }

            var client = httpClientFactory.CreateClient("TestWebAPI");
            // Teraz mając token, chce go wysłać razem z requestem
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
            // WeatherForecast to nazwa kontrolera w WebAPI, który zwraca dane pogodowe. Musi być zgodna z nazwą kontrolera w WebAPI (WeatherForecastController).
            weatherForecastItems = await client.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
        }

        private async Task<JwtToken> AuthenticateAndGetTokenAsync()
        {
            // tak samo jak client z program.cs
            var client = httpClientFactory.CreateClient("TestWebAPI");
            // DODAJE authentykację do żądania HTTP, aby uzyskać dostęp do chronionego zasobu w WebAPI. Musze zrobic post Credential
            var response = await client.PostAsJsonAsync("Auth", new Credential { UserName = "admin", Password = "admin" });
            // Sprawdzam, czy odpowiedź HTTP jest sukcesem (status code 2xx). Jeśli nie, to rzuca wyjątek.
            response.EnsureSuccessStatusCode();
            // Odczytuje zawartość odpowiedzi HTTP jako string, który powinien zawierać token JWT (JSON Web Token) zwrócony przez endpoint "Auth" w WebAPI. 
            string jwt = await response.Content.ReadAsStringAsync();

            // jesli juz mam token to chce go zapisać w session
            HttpContext.Session.SetString("access_token", jwt);

            // musze go desserializowac do formatu objektowego, aby móc go użyć w nagłówku autoryzacji.
            return JsonConvert.DeserializeObject<JwtToken>(jwt) ?? new JwtToken();
        }
    }
}
