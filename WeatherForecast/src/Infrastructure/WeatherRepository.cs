using Core;
using Infrastructure.Utilities;

namespace Infrastructure;

internal class WeatherRepository(WeatherContext weatherContext, IDateTimeWrapper dateTimeWrapper) : IWeatherRepository
{
    public void SaveWeather(WeatherResult weatherResult)
    {
        var weatherReport = new WeatherReport
        {
            City = string.Empty,
            Report = weatherResult.WeatherResponse,
            IsSuccessful = weatherResult.IsSuccessful,
            CreatedAtUtc = dateTimeWrapper.UtcNow,
        };

        weatherContext.WeatherReports.Add(weatherReport);
        weatherContext.SaveChanges();
    }

    public List<string> GetWeatherTrackedCities()
    {
        throw new NotImplementedException();
    }
}