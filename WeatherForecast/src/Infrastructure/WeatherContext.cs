using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

internal class WeatherContext(DbContextOptions<WeatherContext> options) : DbContext(options)
{
    internal DbSet<WeatherReport> WeatherReports { get; set; }
}