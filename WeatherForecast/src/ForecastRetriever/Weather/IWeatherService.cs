namespace ForecastRetriever.Weather;

public interface IWeatherService
{
    Task<WeatherResult> GetWeatherAsync(string city);
}