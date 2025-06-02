using Core;

namespace Infrastructure;

internal class WeatherRepository(WeatherContext weatherContext) : IWeatherRepository
{
    public void SaveWeather(WeatherResult weatherResult)
    {
        var weatherReport = new WeatherReport
        {
            City = string.Empty,
            Report = weatherResult.WeatherResponse,
            IsSuccessful = weatherResult.IsSuccessful,
            CreatedAtUtc = DateTime.UtcNow,
        };

        weatherContext.WeatherReports.Add(weatherReport);
        weatherContext.SaveChanges();
    }
}