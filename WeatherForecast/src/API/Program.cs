using Core;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "FrontEnd",
        policy =>
        {
            policy.WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddInfrastructureServicesForRetrievingWeather(builder.Configuration);
builder.Services.AddCoreServicesForRetrievingWeather();

var app = builder.Build();

app.UseCors("FrontEnd");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();