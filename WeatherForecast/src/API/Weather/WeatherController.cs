using Core.RetrieveWeather;
using Microsoft.AspNetCore.Mvc;

namespace API.Weather;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(IWeatherReaderService weatherReaderService) : ControllerBase
{
    [Route("retrieval-logs")]
    [HttpGet]
    public IActionResult GetRetrievalLogs()
    {
        return Ok(weatherReaderService.GetWeatherUpdateLogs());
    }
}