# Weather App

A weather app for the POS Works development brief. It shows today's weather and a 7-day forecast for any city.

- **Backend:** C# ASP.NET Core Web API (.NET 8)
- **Frontend:** HTML, CSS and JavaScript (works on desktop and mobile)
- **Weather data:** [Open-Meteo](https://open-meteo.com/), a free API that needs no API key
- **Tests:** xUnit

## Features
- Today's temperature, conditions, humidity and wind
- 7-day forecast with rain chance
- City search
- Weather icons (moon at night)
- Today's tip (e.g. sunscreen when UV is high)
- Local emergency number for the country (000 in Australia)

## How to run
1. Open `WeatherApp.sln` in Visual Studio.
2. Set **WeatherApp** as the startup project.
3. Press **F5**. The app opens at https://localhost:7180.
4. API documentation: https://localhost:7180/swagger

Run tests: **Test → Test Explorer → Run All**

## Project structure
```
WeatherApp/
  Controllers/WeatherController.cs   API endpoints, input validation, error handling
  Services/IWeatherService.cs        Interface for the weather service
  Services/WeatherService.cs         Calls Open-Meteo and converts the JSON
  Models/                            Classes for the API JSON and my own results
  Helpers/                           Weather codes, emergency numbers, daily tip
  wwwroot/                           Web page (HTML, CSS, JavaScript)
WeatherApp.Tests/                    Unit tests
```

## Ideas for the future
- Hourly forecast and a temperature chart
- Caching so the same city isn't requested again within a few minutes
- Move API URLs into appsettings.json
- Global error-handling middleware
- A WPF desktop version using the same service
