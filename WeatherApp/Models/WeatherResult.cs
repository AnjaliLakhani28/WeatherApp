namespace WeatherApp.Models
{
    // These are my own simple classes that I send to the front end.
    // The front end does not need to know how the Open-Meteo JSON looks.

    public class WeatherResult
    {
        public string Timezone { get; set; } = "";
        public CurrentWeather Current { get; set; } = new();
        public List<DayForecast> Forecast { get; set; } = new();
        public List<HourForecast> Hourly { get; set; } = new();
        public string Tip { get; set; } = "";
        public string EmergencyNumber { get; set; } = "";
    }

    public class CurrentWeather
    {
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; } = "";
        public string Icon { get; set; } = "";
        public double FeelsLike { get; set; }
    }

    public class DayForecast
    {
        public string Date { get; set; } = "";
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
        public int RainChance { get; set; }
        public double UvIndex { get; set; }
        public string Description { get; set; } = "";
        public string Icon { get; set; } = "";
    }

    public class HourForecast
    {
        public string Time { get; set; } = "";
        public double Temperature { get; set; }
        public int RainChance { get; set; }
        public string Icon { get; set; } = "";
    }
}
