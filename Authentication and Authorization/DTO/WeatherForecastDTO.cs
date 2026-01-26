namespace Authentication_and_Authorization.DTO
{
    public class WeatherForecastDTO
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }
        // DTO Pobiera a nie oblicza wiec nie potrzebuje przeliczać jeszcze raz wartości
        public int TemperatureF { get; set; }

        public string? Summary { get; set; }
    }
}
