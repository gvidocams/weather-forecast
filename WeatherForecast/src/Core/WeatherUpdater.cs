namespace Core;

internal class WeatherUpdater(IWeatherRepository weatherRepository, IWeatherService weatherService) : IWeatherUpdater
{
    public async Task UpdateForTrackedCities()
    {
        var cities = weatherRepository.GetWeatherTrackedCities();
        foreach (var city in cities)
        {
            await GetWeatherAndSave(city);
        }
    }

    private async Task GetWeatherAndSave(string city)
    {
        var weatherResult = await weatherService.GetWeatherAsync(city);
        weatherRepository.SaveWeather(weatherResult);
    }
}