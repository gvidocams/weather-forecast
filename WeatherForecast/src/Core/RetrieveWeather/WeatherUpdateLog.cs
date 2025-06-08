namespace Core.RetrieveWeather;

public class WeatherUpdateLog
{
    public required bool IsUpdateSuccessful { get; set; }
    public required DateTime UpdateDateUtc { get; set; }
    public required string City { get; set; }
}