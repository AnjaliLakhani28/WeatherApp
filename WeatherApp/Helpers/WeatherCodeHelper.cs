namespace WeatherApp.Helpers
{
    // Open-Meteo returns a number (weather code) instead of text.
    // This class turns that number into a description and an icon.
    public static class WeatherCodeHelper
    {
        public static string GetDescription(int code)
        {
            switch (code)
            {
                case 0: return "Clear sky";
                case 1: return "Mainly clear";
                case 2: return "Partly cloudy";
                case 3: return "Overcast";
                case 45:
                case 48: return "Fog";
                case 51:
                case 53:
                case 55: return "Drizzle";
                case 61:
                case 63:
                case 65: return "Rain";
                case 71:
                case 73:
                case 75: return "Snow";
                case 80:
                case 81:
                case 82: return "Showers";
                case 95:
                case 96:
                case 99: return "Thunderstorm";
                default: return "Unknown";
            }
        }

        public static string GetIcon(int code, bool isDay)
        {
            // At night, show a moon instead of a sun for clear skies
            if (!isDay && (code == 0 || code == 1))
            {
                return "🌙";
            }

            switch (code)
            {
                case 0: return "☀️";
                case 1: return "🌤️";
                case 2: return "⛅";
                case 3: return "☁️";
                case 45:
                case 48: return "🌫️";
                case 51:
                case 53:
                case 55:
                case 80:
                case 81: return "🌦️";
                case 61:
                case 63:
                case 65:
                case 82: return "🌧️";
                case 71:
                case 73:
                case 75: return "❄️";
                case 95:
                case 96:
                case 99: return "⛈️";
                default: return "❔";
            }
        }
    }
}
