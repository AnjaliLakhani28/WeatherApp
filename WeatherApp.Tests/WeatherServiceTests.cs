using System.Net;
using WeatherApp.Services;

namespace WeatherApp.Tests
{
    public class WeatherServiceTests
    {
        // Sample JSON in the same shape as the real Open-Meteo response
        private const string SampleWeatherJson = """
        {
          "timezone": "Australia/Brisbane",
          "current": {
            "temperature_2m": 28.4,
            "relative_humidity_2m": 61,
            "wind_speed_10m": 12.4,
            "weather_code": 2,
            "is_day": 1
          },
          "daily": {
            "time": ["2026-11-23", "2026-11-24"],
            "weather_code": [2, 95],
            "temperature_2m_max": [30.2, 27.6],
            "temperature_2m_min": [20.4, 21.1],
            "precipitation_probability_max": [20, null],
            "uv_index_max": [9.1, 6.0]
          },
          "hourly": {
                "time": ["2026-11-23T00:00", "2026-11-23T01:00", "2026-11-23T02:00"],
                "temperature_2m": [27.6, 26.2, 24.9],
                "weather_code": [2, 61, 0],
                "precipitation_probability": [10, 70, null],
                "is_day": [1, 1, 0]
            }

        }
        """;

        private static WeatherService CreateService(FakeHttpMessageHandler handler)
        {
            return new WeatherService(new HttpClient(handler));
        }

        [Fact]
        public async Task GetWeatherAsync_ValidResponse_ReturnsConvertedWeather()
        {
            // Arrange
            var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, SampleWeatherJson);
            var service = CreateService(handler);

            // Act
            var result = await service.GetWeatherAsync(-27.47, 153.03);

            // Assert
            Assert.Equal(28, result.Current.Temperature);         // 28.4 rounded
            Assert.Equal("Partly cloudy", result.Current.Description);
            Assert.Equal(2, result.Forecast.Count);
            Assert.Equal("Thunderstorm", result.Forecast[1].Description);
        }
        [Fact]
        public async Task GetWeatherAsync_ValidResponse_ReturnsHourlyForecast()
        {
            // Arrange
            var service = CreateService(new FakeHttpMessageHandler(HttpStatusCode.OK, SampleWeatherJson));
            //Act
            var result = await service.GetWeatherAsync(-27.47, 153.03);
            // Assert
            Assert.Equal(3, result.Hourly.Count);
            Assert.Equal("2026-11-23T00:00", result.Hourly[0].Time);
            Assert.Equal(28, result.Hourly[0].Temperature); //27.6 rounded
            Assert.Equal(70, result.Hourly[1].RainChance);
            Assert.Equal(0, result.Hourly[2].RainChance); // null becomes 0
            Assert.Equal("🌙",result.Hourly[2].Icon); //clear sky at night shows the moon
        }

        [Fact]
        public async Task GetWeatherAsync_NoHourlyData_ReturnsEmptyHourlyList()
        {
            string jsonWithoutHourly = SampleWeatherJson.Replace("\"hourly\"", "\"not_hourly\"");
            var service = CreateService(new FakeHttpMessageHandler(HttpStatusCode.OK, jsonWithoutHourly));
            var result = await service.GetWeatherAsync(-27.47, 153.03);
            Assert.Empty(result.Hourly);
        }

        [Fact]
        public async Task GetWeatherAsync_NullRainChance_UsesZero()
        {
            var service = CreateService(new FakeHttpMessageHandler(HttpStatusCode.OK, SampleWeatherJson));

            var result = await service.GetWeatherAsync(-27.47, 153.03);

            Assert.Equal(0, result.Forecast[1].RainChance);
        }

        [Fact]
        public async Task GetWeatherAsync_HighUvToday_ReturnsSunscreenTip()
        {
            var service = CreateService(new FakeHttpMessageHandler(HttpStatusCode.OK, SampleWeatherJson));

            var result = await service.GetWeatherAsync(-27.47, 153.03);

            Assert.Contains("sunscreen", result.Tip);
        }

        [Fact]
        public async Task GetWeatherAsync_UsesDotAsDecimalSeparatorInUrl()
        {
            var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, SampleWeatherJson);
            var service = CreateService(handler);

            await service.GetWeatherAsync(-27.47, 153.03);

            Assert.Contains("latitude=-27.47", handler.LastUrl);
        }

        [Fact]
        public async Task GetWeatherAsync_ApiReturnsError_ThrowsException()
        {
            var service = CreateService(new FakeHttpMessageHandler(HttpStatusCode.InternalServerError, "{}"));

            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetWeatherAsync(-27.47, 153.03));
        }

        [Fact]
        public async Task SearchCityAsync_NoResults_ReturnsEmptyList()
        {
            var service = CreateService(new FakeHttpMessageHandler(HttpStatusCode.OK, """{ "generationtime_ms": 0.4 }"""));

            var cities = await service.SearchCityAsync("zzzzzz");

            Assert.Empty(cities);
        }

        [Fact]
        public async Task SearchCityAsync_CityFound_ReturnsCity()
        {
            string json = """
            { "results": [ { "name": "Brisbane", "admin1": "Queensland", "country": "Australia",
                             "country_code": "AU", "latitude": -27.47, "longitude": 153.03 } ] }
            """;
            var service = CreateService(new FakeHttpMessageHandler(HttpStatusCode.OK, json));

            var cities = await service.SearchCityAsync("Brisbane");

            Assert.Single(cities);
            Assert.Equal("Brisbane", cities[0].Name);
            Assert.Equal("AU", cities[0].CountryCode);
        }
    }
}
