using Core;
using Infrastructure.Utilities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Shouldly;

namespace Infrastructure.Tests;

public class WeatherRepositoryTests
{
    private WeatherRepository _weatherRepository;
    private WeatherContext _weatherContext;
    private IDateTimeWrapper _dateTimeWrapper;

    [SetUp]
    public async Task Setup()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<WeatherContext>().UseSqlite(connection).Options;

        _weatherContext = new WeatherContext(options);
        await _weatherContext.Database.EnsureCreatedAsync();

        _dateTimeWrapper = Substitute.For<IDateTimeWrapper>();
        _weatherRepository = new WeatherRepository(_weatherContext, _dateTimeWrapper);
    }

    [TearDown]
    public void TearDown()
    {
        _weatherContext.Dispose();
    }

    [Test]
    public async Task SaveWeatherAsync_ValidWeatherResult_ShouldSaveWeatherData()
    {
        var expectedDate = DateTime.UtcNow;
        _dateTimeWrapper.UtcNow.Returns(expectedDate);

        var weatherResult = new WeatherResult
        {
            IsSuccessful = true,
            WeatherResponse = "ExampleResponse"
        };

        await _weatherRepository.SaveWeatherAsync(weatherResult);

        var result = await _weatherContext.WeatherReports.FirstOrDefaultAsync();
        result.ShouldBeEquivalentTo(new WeatherReport
        {
            Id = 1,
            IsSuccessful = true,
            Report = "ExampleResponse",
            City = string.Empty,
            CreatedAtUtc = expectedDate
        });
    }

    [Test]
    public async Task GetWeatherTrackedCitiesAsync_MultipleTrackedCities_ReturnsAllTrackedCityNames()
    {
        var trackedCities = new List<City>
        {
            new() { Id = 1, CityName = "Riga" },
            new() { Id = 2, CityName = "London" },
            new() { Id = 3, CityName = "Rome" },
        };

        await _weatherContext.Cities.AddRangeAsync(trackedCities);
        await _weatherContext.SaveChangesAsync();

        var result = await _weatherRepository.GetWeatherTrackedCitiesAsync();

        result.ShouldBe(["Riga", "London", "Rome"]);
    }

    [Test]
    public async Task GetWeatherTrackedCitiesAsync_NoTrackedCities_ReturnsEmptyList()
    {
        var result = await _weatherRepository.GetWeatherTrackedCitiesAsync();

        result.ShouldBeEmpty();
    }
}