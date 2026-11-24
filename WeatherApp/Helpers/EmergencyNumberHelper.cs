namespace WeatherApp.Helpers
{
    // Returns the local emergency phone number for a country
    public static class EmergencyNumberHelper
    {
        public static string GetNumber(string? countryCode)
        {
            switch (countryCode?.ToUpper())
            {
                case "AU": return "000";
                case "NZ": return "111";
                case "US":
                case "CA": return "911";
                case "GB": return "999";
                default: return "112";   // works in most countries
            }
        }
    }
}
