using ForecastRetriever.Weather;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Services.Configure<OpenWeatherApiOptions>(builder.Configuration.GetSection("OpenWeatherApi"));

builder.Services.AddHttpClient<IWeatherService>((serviceProvider, client) =>
{
    var openWeatherApiOptions = serviceProvider.GetRequiredService<IOptions<OpenWeatherApiOptions>>().Value;

    client.BaseAddress = new Uri(openWeatherApiOptions.BaseAddress);
});

builder.Build().Run();