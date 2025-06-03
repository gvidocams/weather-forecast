using NSubstitute;

namespace Core.Tests;

public class WeatherUpdaterTests
{
    private WeatherUpdater _weatherUpdater;
    private IWeatherRepository _weatherRepository;
    private IWeatherService _weatherService;

    [SetUp]
    public void Setup()
    {
        _weatherRepository = Substitute.For<IWeatherRepository>();
        _weatherService = Substitute.For<IWeatherService>();
        _weatherUpdater = new WeatherUpdater(_weatherRepository, _weatherService);
    }

    [Test]
    public async Task UpdateForTrackedCities_MultipleCities_ShouldSaveAllWeatherData()
    {
        _weatherRepository
            .GetWeatherTrackedCitiesAsync()
            .Returns(["Riga", "Rome"]);

        var rigaWeatherResult = new WeatherResult
        {
            IsSuccessful = false,
            WeatherResponse = "ErrorResponseForRiga"
        };
        _weatherService.GetWeatherAsync("Riga").Returns(rigaWeatherResult);

        var romeWeatherResult = new WeatherResult
        {
            IsSuccessful = true,
            WeatherResponse = "SuccessResponseForRome"
        };
        _weatherService.GetWeatherAsync("Rome").Returns(romeWeatherResult);

        await _weatherUpdater.UpdateForTrackedCities();

        await _weatherRepository.Received(1).SaveWeatherAsync(rigaWeatherResult);
        await _weatherRepository.Received(1).SaveWeatherAsync(romeWeatherResult);
    }
}