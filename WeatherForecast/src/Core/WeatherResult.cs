namespace Core;

public record WeatherResult
{
    public required bool IsSuccessful { get; init; }
    public required string WeatherResponse { get; init; }
    public required string CityName { get; init; }
}