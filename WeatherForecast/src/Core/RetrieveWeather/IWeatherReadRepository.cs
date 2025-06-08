namespace Core.RetrieveWeather;

public interface IWeatherReadRepository
{
    Task<List<WeatherUpdateLog>> GetWeatherUpdateLogs();
}