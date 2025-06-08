namespace Core.RetrieveWeather;

public class WeatherReaderService(IWeatherReadRepository repository) : IWeatherReaderService
{
    public async Task<List<WeatherUpdateLog>> GetWeatherUpdateLogs() => await repository.GetWeatherUpdateLogs();
}