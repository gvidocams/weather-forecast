using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ForecastRetriever;

public class ForecastRetrieverFunction
{
    private readonly ILogger<ForecastRetrieverFunction> _logger;

    public ForecastRetrieverFunction(ILogger<ForecastRetrieverFunction> logger)
    {
        _logger = logger;
    }

    [Function("ForecastRetrieverFunction")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}