namespace Core;

public interface IWeatherRepository
{
    Task SaveWeatherAsync(WeatherResult weatherResult);
    Task<List<string>> GetWeatherTrackedCitiesAsync();
}