using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DependencyInjection
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherUpdater, WeatherUpdater>();
    }
}