using Microsoft.Extensions.Options;

namespace ForecastRetriever.Weather;

public class OpenWeatherService(HttpClient client, IOptions<OpenWeatherApiOptions> options) : IWeatherService
{
    private readonly OpenWeatherApiOptions _openWeatherApiOptions = options.Value;

    public async Task<WeatherResult> GetWeatherAsync(string city)
    {
        var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={_openWeatherApiOptions.ApiKey}");

        return new WeatherResult
        {
            IsSuccessful = response.IsSuccessStatusCode,
            WeatherResponse = await response.Content.ReadAsStringAsync()
        };
    }
}