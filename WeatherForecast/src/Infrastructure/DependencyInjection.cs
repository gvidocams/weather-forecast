using Core.RetrieveWeather;
using Core.UpdateWeather;
using Infrastructure.OpenWeatherApi;
using Infrastructure.Persistance;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServicesForUpdatingWeather(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IWeatherService, OpenWeatherService>((serviceProvider, client) =>
        {
            var openWeatherApiOptions = serviceProvider.GetRequiredService<IOptions<OpenWeatherApiOptions>>().Value;

            client.BaseAddress = new Uri(openWeatherApiOptions.BaseAddress);
        });

        AddCommonInfrastructureServices(services, configuration);

        services.AddScoped<IWeatherRepository, WeatherRepository>();
        services.AddSingleton<IDateTimeWrapper, DateTimeWrapper>();
    }

    public static void AddInfrastructureServicesForRetrievingWeather(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWeatherReadRepository, WeatherRepository>();

        AddCommonInfrastructureServices(services, configuration);
    }

    private static void AddCommonInfrastructureServices(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<WeatherContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("WeatherDatabase")));
}