namespace Core;

public interface IWeatherRepository
{
    void SaveWeather(WeatherResult weatherResult);
    List<string> GetWeatherTrackedCities();
}