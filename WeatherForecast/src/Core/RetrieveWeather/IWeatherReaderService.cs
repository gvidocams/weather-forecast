namespace Core.RetrieveWeather;

public interface IWeatherReaderService
{
    Task<List<WeatherUpdateLog>> GetWeatherUpdateLogs();
}