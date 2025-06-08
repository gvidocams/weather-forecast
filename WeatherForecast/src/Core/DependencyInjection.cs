using Core.RetrieveWeather;
using Core.UpdateWeather;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DependencyInjection
{
    public static void AddCoreServicesForUpdatingWeather(this IServiceCollection services)
    {
        services.AddScoped<IWeatherUpdater, WeatherUpdater>();
    }

    public static void AddCoreServicesForRetrievingWeather(this IServiceCollection services)
    {
        services.AddScoped<IWeatherReaderService, WeatherReaderService>();
    }
}