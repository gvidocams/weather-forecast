﻿namespace Core.RetrieveWeather;

public interface IWeatherReaderService
{
    Task<List<WeatherUpdateLog>> GetWeatherUpdateLogs();
    Task<List<WeatherUpdateDto>> GetWeatherUpdates(DateTime? date);
}