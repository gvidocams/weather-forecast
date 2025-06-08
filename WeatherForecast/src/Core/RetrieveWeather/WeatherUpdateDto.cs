namespace Core.RetrieveWeather;

public class WeatherUpdateDto
{
    public string City { get; set; }
    public string Country { get; set; }
    public string Temperature { get; set; }
    public string MinTemperature { get; set; }
    public string MaxTemperature { get; set; }
    public DateTime LastUpdateTime { get; set; }
}