using Core.UpdateWeather;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ForecastRetriever;

public class WeatherUpdaterFunction(IWeatherUpdater weatherUpdater, ILogger<WeatherUpdaterFunction> logger)
{
    [Function("ForecastRetrieverFunction")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        logger.LogInformation("{FunctionName} started executing at: {ExecutionStartTime}",
            nameof(WeatherUpdaterFunction), DateTime.Now);

        await weatherUpdater.UpdateForTrackedCities();

        logger.LogInformation("{FunctionName} finished executing at: {ExecutionEndTime}",
            nameof(WeatherUpdaterFunction), DateTime.Now);

        if (myTimer.ScheduleStatus is not null)
        {
            logger.LogInformation("Next {FunctionName} execution scheduled at: {NextExecutionStartTime}",
                nameof(WeatherUpdaterFunction), myTimer.ScheduleStatus.Next);
        }
    }
}