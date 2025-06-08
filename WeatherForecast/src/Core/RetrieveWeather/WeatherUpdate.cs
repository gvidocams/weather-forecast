namespace Core.RetrieveWeather;

public class WeatherUpdate
{
    public required string City { get; set; }
    public required DateTime CreatedOnUtc { get; set; }
    public required string WeatherReport { get; set; }
}