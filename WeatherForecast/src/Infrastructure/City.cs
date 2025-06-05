namespace Infrastructure;

internal class City
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<WeatherReport> WeatherReports { get; set; }
}