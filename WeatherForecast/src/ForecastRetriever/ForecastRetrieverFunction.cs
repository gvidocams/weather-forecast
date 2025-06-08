using Core.UpdateWeather;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ForecastRetriever;

public class ForecastRetrieverFunction(IWeatherUpdater weatherUpdater, ILogger<ForecastRetrieverFunction> logger)
{
    [Function("ForecastRetrieverFunction")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        logger.LogInformation("{FunctionName} started executing at: {ExecutionStartTime}",
            nameof(ForecastRetrieverFunction), DateTime.Now);

        await weatherUpdater.UpdateForTrackedCities();

        logger.LogInformation("{FunctionName} finished executing at: {ExecutionEndTime}",
            nameof(ForecastRetrieverFunction), DateTime.Now);

        if (myTimer.ScheduleStatus is not null)
        {
            logger.LogInformation("Next {FunctionName} execution scheduled at: {NextExecutionStartTime}",
                nameof(ForecastRetrieverFunction), myTimer.ScheduleStatus.Next);
        }
    }
}