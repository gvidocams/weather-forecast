using Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ForecastRetriever;

public class ForecastRetrieverFunction(IWeatherUpdater weatherUpdater, ILogger<ForecastRetrieverFunction> logger)
{
    [Function("ForecastRetrieverFunction")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        await weatherUpdater.UpdateForTrackedCities();

        if (myTimer.ScheduleStatus is not null)
        {
            logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}