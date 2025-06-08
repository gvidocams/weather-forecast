namespace Core.UpdateWeather;

public interface IWeatherRepository
{
    Task SaveWeatherAsync(WeatherResult weatherResult);
    Task<List<string>> GetWeatherTrackedCitiesAsync();
}