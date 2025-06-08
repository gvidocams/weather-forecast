namespace Infrastructure.Persistance.Entities;

internal class CityEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<WeatherReportEntity> WeatherReports { get; set; }
}