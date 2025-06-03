using Core;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

internal class WeatherRepository(WeatherContext weatherContext, IDateTimeWrapper dateTimeWrapper) : IWeatherRepository
{
    public async Task SaveWeatherAsync(WeatherResult weatherResult)
    {
        var weatherReport = new WeatherReport
        {
            City = string.Empty,
            Report = weatherResult.WeatherResponse,
            IsSuccessful = weatherResult.IsSuccessful,
            CreatedAtUtc = dateTimeWrapper.UtcNow,
        };

        await weatherContext.WeatherReports.AddAsync(weatherReport);
        await weatherContext.SaveChangesAsync();
    }

    public Task<List<string>> GetWeatherTrackedCitiesAsync()
    {
        return weatherContext.Cities
            .Select(city => city.CityName)
            .ToListAsync();
    }
}