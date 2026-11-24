# Weather App

A weather app built for the POS Works development brief. It shows today's weather, the next 12 hours and a 7-day forecast for any city in the world.

**Live demo:** http://anjali-weather-app.runasp.net

- **Backend:** C# ASP.NET Core Web API (.NET 8)
- **Frontend:** HTML, CSS and JavaScript (works on desktop and mobile)
- **Weather data:** [Open-Meteo](https://open-meteo.com/), a free API that needs no API key
- **Tests:** xUnit

## Features
- Current weather: temperature, feels like, humidity and wind
- Hourly forecast for the next 12 hours, with rain chance
- 7-day forecast with "Today", rain chance and min/max temperatures
- Search for any city worldwide
- Weather icons (moon at night)
- Today's tip based on the weather (e.g. sunscreen when UV is high, umbrella when rain is likely)
- Local emergency number for the selected country (000 in Australia, 111 in New Zealand, 911 in the US and Canada, 999 in the UK, 112 elsewhere)

## How to run
1. Clone this repository.
2. Open `WeatherApp.sln` in Visual Studio 2022 or later (needs the .NET 8 SDK).
3. Set **WeatherApp** as the startup project.
4. Press **F5**. The app opens in the browser.
5. API documentation (Swagger) is at `/swagger` when running locally.

## API endpoints
| Endpoint | Description |
|---|---|
| `GET /api/weather?latitude=-27.47&longitude=153.03&countryCode=AU` | Current weather, hourly and 7-day forecast, tip and emergency number |
| `GET /api/weather/search?city=Brisbane` | Search for a city by name |

Invalid input returns **400 Bad Request**. If the weather service is unavailable, the API returns **502 Bad Gateway** with a friendly message.

## Tests
28 xUnit unit tests cover the helpers (weather codes, emergency numbers, tips) and the weather service. The service tests use a fake HTTP handler, so they don't call the real internet.

Run them from **Test - Test Explorer - Run All**.

## Project structure
```
WeatherApp/
  Controllers/WeatherController.cs   API endpoints, input validation, error handling
  Services/IWeatherService.cs        Interface for the weather service
  Services/WeatherService.cs         Calls Open-Meteo and converts the JSON
  Models/                            Classes for the API JSON and my own results
  Helpers/                           Weather codes and icons, emergency numbers, daily tip
  wwwroot/                           Web page (HTML, CSS, JavaScript)
WeatherApp.Tests/                    Unit tests
```

## Security
- Input validation on all API parameters
- User input is URL-encoded before calling the weather API
- The page uses `textContent` instead of `innerHTML` to prevent XSS
- Errors are logged on the server; users only see a friendly message
- No API keys or publish credentials in the repository

## Deployment
Hosted on MonsterASP.NET, using Web Deploy from Visual Studio. The free hosting plan doesn't include an SSL certificate, so the live site uses HTTP. Locally, the app uses HTTPS.

## Future ideas
- Temperature chart for the week (e.g. with Chart.js)
- Weather-based page backgrounds
- Caching, so the same city isn't requested again within a few minutes
- Move API URLs into appsettings.json
- Global error-handling middleware
- A WPF desktop version that reuses the same service

## Author
Anjali Lakhani ([AnjaliLakhani28](https://github.com/AnjaliLakhani28))
