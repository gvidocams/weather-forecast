namespace Core.UpdateWeather;

public interface IWeatherService
{
    Task<WeatherResult> GetWeatherAsync(string city);
}