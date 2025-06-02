namespace Infrastructure;

internal class WeatherReport
{
    public int Id { get; set; }
    public required string City { get; set; }
    public required string Report { get; set; }
    public required bool IsSuccessful { get; set; }
    public required DateTime CreatedAtUtc { get; set; }
}