# Getting started

## Pre-requisites
- Added API key in WeatherForecast/src/WeatherUpdater/local.settings.json file under OpenWeatherApi:ApiKey configuration
- Installed .NET 9 SDK or later
- Azure Functions Core Tools v4
- Azurite Emulator

## Running the application

1. Build the solution
```shell
dotnet build
```

2. Run the API

```shell
dotnet run --project WeatherForecast/src/API/API.csproj
```

3. Run the Weather Updater service
```shell
npx azurite
dotnet run --project WeatherForecast/src/WeatherUpdater/WeatherUpdater.csproj
```

4. Run the front end application
```shell
cd frontend
npm install
npm start
```