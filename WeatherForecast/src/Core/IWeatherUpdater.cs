namespace Core;

internal interface IWeatherUpdater
{
    Task UpdateForTrackedCities();
}