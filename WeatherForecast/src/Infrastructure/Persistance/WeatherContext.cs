using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

internal class WeatherContext(DbContextOptions<WeatherContext> options) : DbContext(options)
{
    internal DbSet<WeatherReport> WeatherReports { get; set; }
    internal DbSet<City> TrackedCities { get; set; }
}