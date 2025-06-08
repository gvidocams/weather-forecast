namespace Core.UpdateWeather;

public interface IWeatherUpdater
{
    Task UpdateForTrackedCities();
}