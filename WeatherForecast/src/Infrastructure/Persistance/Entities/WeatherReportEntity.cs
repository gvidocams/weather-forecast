namespace Infrastructure.Persistance.Entities;

internal class WeatherReportEntity
{
    public int Id { get; set; }
    public required string Report { get; set; }
    public required bool IsSuccessful { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
    public CityEntity City { get; set; }
}