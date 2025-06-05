using Core;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

internal class WeatherRepository(WeatherContext weatherContext, IDateTimeWrapper dateTimeWrapper) : IWeatherRepository
{
    public async Task SaveWeatherAsync(WeatherResult weatherResult)
    {
        var city = await weatherContext.Cities.FirstAsync(city => city.Name == weatherResult.CityName);

        var weatherReport = new WeatherReport
        {
            City = city,
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
            .Select(city => city.Name)
            .ToListAsync();
    }
}