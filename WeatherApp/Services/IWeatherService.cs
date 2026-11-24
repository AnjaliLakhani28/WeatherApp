using WeatherApp.Models;

namespace WeatherApp.Services
{
    // Interface for the weather service.
    // The controller uses this interface, so I can swap the weather provider later
    // or use a fake one in tests.
    public interface IWeatherService
    {
        Task<WeatherResult> GetWeatherAsync(double latitude, double longitude);
        Task<List<City>> SearchCityAsync(string cityName);
    }
}
