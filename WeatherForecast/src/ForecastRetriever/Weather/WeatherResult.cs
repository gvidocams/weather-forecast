namespace ForecastRetriever.Weather;

public record WeatherResult
{
    public required bool IsSuccessful { get; init; }
    public required string WeatherResponse { get; init; }
}