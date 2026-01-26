using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];


        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            // I tak poka¿e sie wszystko co jest w WeatherForecast, czyli w³¹cznie z TemperatureF
            // Jeœli tego nie chcemy, to musimy stworzyæ osobn¹ klasê DTO i zwracaæ j¹ zamiast oryginalnej klasy
            // public record WeatherForecastDto(
            // DateOnly Date,
            // int TemperatureC,
            // string? Summary);

            // albo u¿yæ atrybutu [JsonIgnore] na w³aœciwoœci, któr¹ chcemy pomin¹æ w serializacji JSON
        }
    }
}
