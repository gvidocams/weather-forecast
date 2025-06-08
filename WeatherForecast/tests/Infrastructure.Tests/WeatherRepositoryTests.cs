using Core;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Infrastructure.Tests;

public class WeatherRepositoryTests
{
    private WeatherRepository _weatherRepository;
    private WeatherContext _weatherContext;

    [SetUp]
    public async Task Setup()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<WeatherContext>().UseSqlite(connection).Options;

        _weatherContext = new WeatherContext(options);
        await _weatherContext.Database.EnsureCreatedAsync();

        _weatherRepository = new WeatherRepository(_weatherContext);
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
        var trackedCity = new CityEntity { Id = 1, Name = "Riga" };
        await _weatherContext.TrackedCities.AddAsync(trackedCity);
        await _weatherContext.SaveChangesAsync();

        var weatherResult = new WeatherResult
        {
            IsSuccessful = true,
            WeatherResponse = "ExampleResponse",
            CityName = "Riga",
            CreatedAtUtc = expectedDate,
        };

        await _weatherRepository.SaveWeatherAsync(weatherResult);

        var result = await _weatherContext.WeatherReports.FirstOrDefaultAsync();
        result.ShouldBeEquivalentTo(new WeatherReportEntity
        {
            Id = 1,
            IsSuccessful = true,
            Report = "ExampleResponse",
            City = trackedCity,
            CreatedAtUtc = expectedDate
        });
    }

    [Test]
    public async Task GetWeatherTrackedCitiesAsync_MultipleTrackedCities_ReturnsAllTrackedCityNames()
    {
        var trackedCities = new List<CityEntity>
        {
            new() { Id = 1, Name = "Riga" },
            new() { Id = 2, Name = "London" },
            new() { Id = 3, Name = "Rome" },
        };

        await _weatherContext.TrackedCities.AddRangeAsync(trackedCities);
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

    [Test]
    public async Task GetWeatherUpdates_NoDate_ReturnsLatestWeatherUpdateForEachCity()
    {
        var city1 = new CityEntity { Id = 1, Name = "Riga" };
        var city2 = new CityEntity { Id = 2, Name = "London" };
        await _weatherContext.TrackedCities.AddRangeAsync([city1, city2]);

        var now = DateTime.UtcNow;
        var yesterday = now.AddDays(-1);

        var weatherReports = new List<WeatherReportEntity>
        {
            new() { Id = 1, City = city1, Report = "Riga Today", IsSuccessful = true, CreatedAtUtc = now },
            new() { Id = 2, City = city1, Report = "Riga Yesterday", IsSuccessful = true, CreatedAtUtc = yesterday },
            new() { Id = 3, City = city2, Report = "London Today", IsSuccessful = true, CreatedAtUtc = now },
            new() { Id = 4, City = city2, Report = "London Yesterday", IsSuccessful = true, CreatedAtUtc = yesterday },
        };

        await _weatherContext.WeatherReports.AddRangeAsync(weatherReports);
        await _weatherContext.SaveChangesAsync();

        var result = await _weatherRepository.GetWeatherUpdates(null);

        result.Count.ShouldBe(2);
        result.ShouldContain(u => u.City == "Riga" && u.WeatherReport == "Riga Today" && u.CreatedOnUtc == now);
        result.ShouldContain(u => u.City == "London" && u.WeatherReport == "London Today" && u.CreatedOnUtc == now);
    }

    [Test]
    public async Task GetWeatherUpdates_WithDate_ReturnsLatestWeatherUpdateBeforeDate()
    {
        var city = new CityEntity { Id = 1, Name = "Riga" };
        await _weatherContext.TrackedCities.AddAsync(city);

        var now = DateTime.UtcNow;
        var yesterday = now.AddDays(-1);
        var twoDaysAgo = now.AddDays(-2);

        var weatherReports = new List<WeatherReportEntity>
        {
            new() { Id = 1, City = city, Report = "Riga Today", IsSuccessful = true, CreatedAtUtc = now },
            new() { Id = 2, City = city, Report = "Riga Yesterday", IsSuccessful = true, CreatedAtUtc = yesterday },
            new() { Id = 3, City = city, Report = "Riga Two Days Ago", IsSuccessful = true, CreatedAtUtc = twoDaysAgo },
        };

        await _weatherContext.WeatherReports.AddRangeAsync(weatherReports);
        await _weatherContext.SaveChangesAsync();

        var result = await _weatherRepository.GetWeatherUpdates(yesterday);

        result.Count.ShouldBe(1);
        result.ShouldContain(u =>
            u.City == "Riga" && u.WeatherReport == "Riga Yesterday" && u.CreatedOnUtc == yesterday);
    }

    [Test]
    public async Task GetWeatherUpdates_OnlySuccessfulReports_FiltersOutUnsuccessfulWeatherReports()
    {
        var city = new CityEntity { Id = 1, Name = "Riga" };
        await _weatherContext.TrackedCities.AddAsync(city);

        var now = DateTime.UtcNow;

        var weatherReports = new List<WeatherReportEntity>
        {
            new() { Id = 1, City = city, Report = "Successful Report", IsSuccessful = true, CreatedAtUtc = now },
            new()
            {
                Id = 2, City = city, Report = "Unsuccessful Report", IsSuccessful = false,
                CreatedAtUtc = now.AddMinutes(1)
            },
        };

        await _weatherContext.WeatherReports.AddRangeAsync(weatherReports);
        await _weatherContext.SaveChangesAsync();

        var result = await _weatherRepository.GetWeatherUpdates(null);

        result.Count.ShouldBe(1);
        result.ShouldContain(u => u.City == "Riga" && u.WeatherReport == "Successful Report" && u.CreatedOnUtc == now);
    }

    [Test]
    public async Task GetWeatherUpdates_NoSuccessfulReports_ReturnsEmptyList()
    {
        var city = new CityEntity { Id = 1, Name = "Riga" };
        await _weatherContext.TrackedCities.AddAsync(city);

        var now = DateTime.UtcNow;

        var weatherReports = new List<WeatherReportEntity>
        {
            new() { Id = 1, City = city, Report = "Unsuccessful Report 1", IsSuccessful = false, CreatedAtUtc = now },
            new()
            {
                Id = 2, City = city, Report = "Unsuccessful Report 2", IsSuccessful = false,
                CreatedAtUtc = now.AddMinutes(1)
            },
        };

        await _weatherContext.WeatherReports.AddRangeAsync(weatherReports);
        await _weatherContext.SaveChangesAsync();

        var result = await _weatherRepository.GetWeatherUpdates(null);

        result.ShouldBeEmpty();
    }

    [Test]
    public async Task GetWeatherUpdates_MultipleReportsForCityBeforeDate_ReturnsOnlyLatestBeforeDate()
    {
        var city = new CityEntity { Id = 1, Name = "Riga" };
        await _weatherContext.TrackedCities.AddAsync(city);

        var now = DateTime.UtcNow;
        var yesterday = now.AddDays(-1);
        var twoDaysAgo = now.AddDays(-2);
        var threeDaysAgo = now.AddDays(-3);
        var cutoffDate = yesterday.AddHours(12);

        var weatherReports = new List<WeatherReportEntity>
        {
            new() { Id = 1, City = city, Report = "Today", IsSuccessful = true, CreatedAtUtc = now },
            new()
            {
                Id = 2, City = city, Report = "Yesterday Evening", IsSuccessful = true,
                CreatedAtUtc = yesterday.AddHours(18)
            },
            new()
            {
                Id = 3, City = city, Report = "Yesterday Morning", IsSuccessful = true,
                CreatedAtUtc = yesterday.AddHours(8)
            },
            new() { Id = 4, City = city, Report = "Two Days Ago", IsSuccessful = true, CreatedAtUtc = twoDaysAgo },
            new() { Id = 5, City = city, Report = "Three Days Ago", IsSuccessful = true, CreatedAtUtc = threeDaysAgo },
        };

        await _weatherContext.WeatherReports.AddRangeAsync(weatherReports);
        await _weatherContext.SaveChangesAsync();

        var result = await _weatherRepository.GetWeatherUpdates(cutoffDate);

        result.Count.ShouldBe(1);
        result.ShouldContain(u =>
            u.City == "Riga" && u.WeatherReport == "Yesterday Morning" && u.CreatedOnUtc == yesterday.AddHours(8));
    }
}