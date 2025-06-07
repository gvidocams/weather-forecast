namespace Infrastructure.OpenWeatherApi;

public class OpenWeatherApiOptions
{
    public required string BaseAddress { get; init; }
    public required string ApiKey { get; init; }
}