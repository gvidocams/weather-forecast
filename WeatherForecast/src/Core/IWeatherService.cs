namespace Core;

public interface IWeatherService
{
    Task<WeatherResult> GetWeatherAsync(string city);
}