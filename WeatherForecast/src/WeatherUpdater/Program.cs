using Core;
using Infrastructure;
using Infrastructure.OpenWeatherApi;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.Configure<OpenWeatherApiOptions>(builder.Configuration.GetSection("OpenWeatherApi"));

builder.Services.AddInfrastructureServicesForUpdatingWeather(builder.Configuration);
builder.Services.AddCoreServicesForUpdatingWeather();

await builder.Build().RunAsync();