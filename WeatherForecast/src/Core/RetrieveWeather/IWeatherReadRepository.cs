namespace Core.RetrieveWeather;

public interface IWeatherReadRepository
{
    Task<List<WeatherUpdateLog>> GetWeatherUpdateLogs();
    Task<List<WeatherUpdate>> GetWeatherUpdates(DateTime? date);
}