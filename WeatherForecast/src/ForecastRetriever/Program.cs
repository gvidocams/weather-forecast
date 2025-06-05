using Core;
using Infrastructure;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.Configure<OpenWeatherApiOptions>(builder.Configuration.GetSection("OpenWeatherApi"));

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCoreServices();

await builder.Build().RunAsync();