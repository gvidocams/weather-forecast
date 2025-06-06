namespace Core;

internal class WeatherUpdater(IWeatherRepository weatherRepository, IWeatherService weatherService) : IWeatherUpdater
{
    public async Task UpdateForTrackedCities()
    {
        var cities = await weatherRepository.GetWeatherTrackedCitiesAsync();
        
        //logger.LogInformation("Retrieved tracked cities {Cities} for weather update", string.Join(", ", cities));
        
        foreach (var city in cities)
        {
            await GetWeatherAndSave(city);
        }
    }

    private async Task GetWeatherAndSave(string city)
    {
        var weatherResult = await weatherService.GetWeatherAsync(city);
        
        await weatherRepository.SaveWeatherAsync(weatherResult);
    }
}