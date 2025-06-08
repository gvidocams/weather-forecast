using Core.RetrieveWeather;
using Microsoft.AspNetCore.Mvc;

namespace API.Weather;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(IWeatherReaderService weatherReaderService) : ControllerBase
{
    [Route("retrieval-logs")]
    [HttpGet]
    public async Task<IActionResult> GetRetrievalLogs() => Ok(await weatherReaderService.GetWeatherUpdateLogs());

    [Route("updates")]
    [HttpGet]
    public async Task<IActionResult> GetWeatherUpdates([FromQuery] DateTime? date = null)
    {
        return Ok(await weatherReaderService.GetWeatherUpdates(date));
    }
}