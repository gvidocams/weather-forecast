﻿using System.Net;
using Core;
using Infrastructure.OpenWeatherApi;
using Infrastructure.Utilities;
using Microsoft.Extensions.Options;
using NSubstitute;
using Shouldly;

namespace Infrastructure.Tests;

public class OpenWeatherServiceTests
{
    private const string ApiKey = "TestApiKey";

    private OpenWeatherService _openWeatherService;
    private MockHttpMessageHandler _httpMessageHandler;
    private HttpClient _httpClient;
    private IDateTimeWrapper _dateTimeWrapper;

    [SetUp]
    public void Setup()
    {
        var options = Options.Create(new OpenWeatherApiOptions
        {
            ApiKey = ApiKey,
            BaseAddress = string.Empty,
        });

        _httpMessageHandler = Substitute.ForPartsOf<MockHttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandler)
        {
            BaseAddress = new Uri("https://example.com")
        };
        _dateTimeWrapper = Substitute.For<IDateTimeWrapper>();

        _openWeatherService = new OpenWeatherService(_httpClient, _dateTimeWrapper, options);
    }

    [TearDown]
    public void TearDown()
    {
        _httpMessageHandler.Dispose();
        _httpClient.Dispose();
    }

    [Test]
    public async Task GetWeatherAsync_SuccessfulResponse_ReturnsSuccessfulWeatherResult()
    {
        const string expectedContent = "TestContent";

        SetupGetAsync(HttpStatusCode.OK, expectedContent);
        
        var expectedCreationDate = DateTime.UtcNow;
        _dateTimeWrapper.UtcNow.Returns(expectedCreationDate);

        var response = await _openWeatherService.GetWeatherAsync("Riga");

        response.ShouldBeEquivalentTo(new WeatherResult
        {
            IsSuccessful = true,
            WeatherResponse = expectedContent,
            CityName = "Riga",
            CreatedAtUtc = expectedCreationDate,
        });
    }

    [Test]
    public async Task GetWeatherAsync_FailureResponse_ReturnsUnsuccessfulWeatherResult()
    {
        const string cityName = "Riga";

        SetupGetAsync(HttpStatusCode.BadRequest, string.Empty);
        
        var expectedCreationDate = DateTime.UtcNow;
        _dateTimeWrapper.UtcNow.Returns(expectedCreationDate);

        var response = await _openWeatherService.GetWeatherAsync(cityName);

        response.ShouldBeEquivalentTo(new WeatherResult
        {
            IsSuccessful = false,
            WeatherResponse = string.Empty,
            CityName = cityName,
            CreatedAtUtc = expectedCreationDate,
        });
    }

    [Test]
    public async Task GetWeatherAsync_GetWeatherInRiga_ShouldCallCorrectUri()
    {
        const string cityName = "Riga";

        SetupGetAsync(HttpStatusCode.OK, string.Empty);

        await _openWeatherService.GetWeatherAsync("Riga");

        await _httpMessageHandler.Received(1).PublicSendAsync(
            Arg.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Get &&
                request.RequestUri!.ToString() == $"https://example.com/data/2.5/weather?q={cityName}&appid={ApiKey}"),
            Arg.Any<CancellationToken>());
    }

    private void SetupGetAsync(HttpStatusCode expectedStatusCode, string expectedContent) =>
        _httpMessageHandler
            .PublicSendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
            .Returns(new HttpResponseMessage(expectedStatusCode)
            {
                Content = new StringContent(expectedContent)
            });
}