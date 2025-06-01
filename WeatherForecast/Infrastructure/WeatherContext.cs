using Microsoft.EntityFrameworkCore;

namespace Database;

internal class WeatherContext(DbContextOptions<WeatherContext> options) : DbContext(options)
{
    internal DbSet<WeatherReport> WeatherReports { get; set; }
}