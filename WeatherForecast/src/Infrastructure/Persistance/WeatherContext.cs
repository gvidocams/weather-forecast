using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

internal class WeatherContext(DbContextOptions<WeatherContext> options) : DbContext(options)
{
    internal DbSet<WeatherReportEntity> WeatherReports { get; set; }
    internal DbSet<CityEntity> TrackedCities { get; set; }
}