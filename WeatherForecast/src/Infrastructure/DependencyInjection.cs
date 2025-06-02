using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IWeatherService, OpenWeatherService>((serviceProvider, client) =>
        {
            var openWeatherApiOptions = serviceProvider.GetRequiredService<IOptions<OpenWeatherApiOptions>>().Value;

            client.BaseAddress = new Uri(openWeatherApiOptions.BaseAddress);
        });

        services.AddDbContext<WeatherContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("WeatherDatabase")));

        services.AddScoped<IWeatherRepository, WeatherRepository>();
    }
}