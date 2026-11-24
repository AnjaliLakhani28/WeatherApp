using System.Globalization;
using WeatherApp.Helpers;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    // Calls the Open-Meteo API (free, no API key needed) and converts the result
    // into my own WeatherResult model.
    public class WeatherService : IWeatherService
    {
        private const string ForecastUrl = "https://api.open-meteo.com/v1/forecast";
        private const string SearchUrl = "https://geocoding-api.open-meteo.com/v1/search";

        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherResult> GetWeatherAsync(double latitude, double longitude)
        {
            // InvariantCulture makes sure the number uses "." (e.g. -27.47) on every computer
            string lat = latitude.ToString(CultureInfo.InvariantCulture);
            string lon = longitude.ToString(CultureInfo.InvariantCulture);

            string url = $"{ForecastUrl}?latitude={lat}&longitude={lon}" +
                         "&current=temperature_2m,apparent_temperature,relative_humidity_2m,wind_speed_10m,weather_code,is_day" +
                         "&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max,uv_index_max" +
                         "&timezone=auto";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Weather API returned status code {(int)response.StatusCode}");
            }

            OpenMeteoResponse? data = await response.Content.ReadFromJsonAsync<OpenMeteoResponse>();

            if (data == null || data.Current == null || data.Daily == null)
            {
                throw new Exception("Weather API returned incomplete data");
            }

            return ConvertToWeatherResult(data);
        }

        public async Task<List<City>> SearchCityAsync(string cityName)
        {
            // EscapeDataString makes the city name safe to put in a URL (e.g. spaces become %20)
            string url = $"{SearchUrl}?name={Uri.EscapeDataString(cityName)}&count=5";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"City search returned status code {(int)response.StatusCode}");
            }

            GeocodingResponse? data = await response.Content.ReadFromJsonAsync<GeocodingResponse>();

            // If nothing is found, the API leaves out "results", so return an empty list
            if (data == null || data.Results == null)
            {
                return new List<City>();
            }

            return data.Results;
        }

        // Converts the Open-Meteo data into the simple model the front end uses
        private static WeatherResult ConvertToWeatherResult(OpenMeteoResponse data)
        {
            var result = new WeatherResult();
            result.Timezone = data.Timezone;

            bool isDay = data.Current!.IsDay == 1;
            result.Current = new CurrentWeather
            {
                Temperature = Math.Round(data.Current.Temperature),
                Humidity = data.Current.Humidity,
                WindSpeed = Math.Round(data.Current.WindSpeed),
                Description = WeatherCodeHelper.GetDescription(data.Current.WeatherCode),
                Icon = WeatherCodeHelper.GetIcon(data.Current.WeatherCode, isDay),
                FeelsLike = Math.Round(data.Current.FeelsLike)
            };

            OpenMeteoDaily daily = data.Daily!;
            for (int i = 0; i < daily.Dates.Count; i++)
            {
                var day = new DayForecast
                {
                    Date = daily.Dates[i],
                    MaxTemp = Math.Round(daily.MaxTemps[i]),
                    MinTemp = Math.Round(daily.MinTemps[i]),
                    RainChance = (int)(daily.RainChances[i] ?? 0),   // use 0 if the value is null
                    UvIndex = daily.UvIndexes[i] ?? 0,
                    Description = WeatherCodeHelper.GetDescription(daily.WeatherCodes[i]),
                    Icon = WeatherCodeHelper.GetIcon(daily.WeatherCodes[i], true)
                };
                result.Forecast.Add(day);
            }

            if (result.Forecast.Count > 0)
            {
                result.Tip = WeatherTipHelper.GetTip(result.Forecast[0]);
            }

            return result;
        }
    }
}
