namespace Core;

public interface IWeatherUpdater
{
    Task UpdateForTrackedCities();
}