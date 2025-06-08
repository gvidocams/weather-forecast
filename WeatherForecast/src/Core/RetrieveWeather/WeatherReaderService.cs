using Newtonsoft.Json.Linq;

namespace Core.RetrieveWeather;

public class WeatherReaderService(IWeatherReadRepository repository) : IWeatherReaderService
{
    public async Task<List<WeatherUpdateLog>> GetWeatherUpdateLogs() => await repository.GetWeatherUpdateLogs();

    public async Task<List<WeatherUpdateDto>> GetWeatherUpdates(DateTime? date)
    {
        var weatherUpdates = await repository.GetWeatherUpdates(date);

        return weatherUpdates.Select(weatherUpdate =>
        {
            var payload = JObject.Parse(weatherUpdate.WeatherReport);

            return new WeatherUpdateDto
            {
                Country = payload["sys"]["country"].ToString(),
                City = weatherUpdate.City,
                Temperature = payload["main"]["temp"].ToString(),
                MinTemperature = payload["main"]["temp_min"].ToString(),
                MaxTemperature = payload["main"]["temp_max"].ToString(),
                LastUpdateTime = weatherUpdate.CreatedOnUtc
            };
        }).ToList();
    }
}