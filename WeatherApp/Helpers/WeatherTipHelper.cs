using WeatherApp.Models;

namespace WeatherApp.Helpers
{
    // Gives a simple tip for today based on the forecast
    public static class WeatherTipHelper
    {
        public static string GetTip(DayForecast today)
        {
            if (today.UvIndex >= 8)
            {
                return "Very high UV today - remember sunscreen and a hat.";
            }

            if (today.RainChance >= 60)
            {
                return "Rain is likely today - take an umbrella.";
            }

            if (today.MaxTemp >= 35)
            {
                return "Very hot today - drink plenty of water.";
            }

            return "Enjoy your day!";
        }
    }
}
