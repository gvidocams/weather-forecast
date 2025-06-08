using Core;
using Core.RetrieveWeather;
using Core.UpdateWeather;
using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

internal class WeatherRepository(WeatherContext weatherContext) : IWeatherRepository, IWeatherReadRepository
{
    public async Task SaveWeatherAsync(WeatherResult weatherResult)
    {
        var city = await weatherContext.TrackedCities.FirstAsync(city => city.Name == weatherResult.CityName);

        var weatherReport = new WeatherReportEntity
        {
            City = city,
            Report = weatherResult.WeatherResponse,
            IsSuccessful = weatherResult.IsSuccessful,
            CreatedAtUtc = weatherResult.CreatedAtUtc,
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

    public Task<List<WeatherUpdateLog>> GetWeatherUpdateLogs() =>
        weatherContext.WeatherReports
            .Select(weatherReport => new WeatherUpdateLog
            {
                IsUpdateSuccessful = weatherReport.IsSuccessful,
                City = weatherReport.City.Name,
                UpdateDateUtc = weatherReport.CreatedAtUtc,
            })
            .ToListAsync();
}