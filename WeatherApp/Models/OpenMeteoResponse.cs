using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    // These classes match the JSON that the Open-Meteo API sends back.
    // [JsonPropertyName] links the JSON field name to my C# property name.

    public class OpenMeteoResponse
    {
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = "";

        [JsonPropertyName("current")]
        public OpenMeteoCurrent? Current { get; set; }

        [JsonPropertyName("daily")]
        public OpenMeteoDaily? Daily { get; set; }

        [JsonPropertyName("hourly")]
        public OpenMeteoHourly? Hourly { get; set; }
    }

    public class OpenMeteoCurrent
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public int Humidity { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed { get; set; }

        [JsonPropertyName("weather_code")]
        public int WeatherCode { get; set; }

        [JsonPropertyName("is_day")]
        public int IsDay { get; set; }

        [JsonPropertyName("apparent_temperature")]
        public double FeelsLike { get; set; }
    }

    public class OpenMeteoDaily
    {
        [JsonPropertyName("time")]
        public List<string> Dates { get; set; } = new();

        [JsonPropertyName("weather_code")]
        public List<int> WeatherCodes { get; set; } = new();

        [JsonPropertyName("temperature_2m_max")]
        public List<double> MaxTemps { get; set; } = new();

        [JsonPropertyName("temperature_2m_min")]
        public List<double> MinTemps { get; set; } = new();

        // Nullable because the API sometimes returns null for these
        [JsonPropertyName("precipitation_probability_max")]
        public List<double?> RainChances { get; set; } = new();

        [JsonPropertyName("uv_index_max")]
        public List<double?> UvIndexes { get; set; } = new();
    }

    public class OpenMeteoHourly
    {
        [JsonPropertyName("time")]
        public List<string> Times { get; set; } = new();
        [JsonPropertyName("temperature_2m")]
        public List<double> Temperatures { get; set; } = new();
        [JsonPropertyName("weather_code")]
        public List<int> WeatherCodes { get; set; } = new();
        [JsonPropertyName("precipitation_probability")]
        public List<double?> RainChances { get; set; } = new();
        [JsonPropertyName("is_day")]
        public List<int> IsDay { get; set; } = new();
    }
    // Response from the city search API
    public class GeocodingResponse
    {
        [JsonPropertyName("results")]
        public List<City>? Results { get; set; }
    }
}
