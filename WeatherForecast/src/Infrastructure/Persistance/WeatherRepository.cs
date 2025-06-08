using Core;
using Core.UpdateWeather;
using Infrastructure.Persistance.Entities;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

internal class WeatherRepository(WeatherContext weatherContext, IDateTimeWrapper dateTimeWrapper) : IWeatherRepository
{
    public async Task SaveWeatherAsync(WeatherResult weatherResult)
    {
        var city = await weatherContext.TrackedCities.FirstAsync(city => city.Name == weatherResult.CityName);

        var weatherReport = new WeatherReportEntity
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
        return weatherContext.TrackedCities
            .Select(city => city.Name)
            .ToListAsync();
    }
}