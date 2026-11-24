using Microsoft.AspNetCore.Mvc;
using WeatherApp.Helpers;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        // The weather service is passed in by dependency injection (registered in Program.cs)
        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        // GET api/weather?latitude=-27.47&longitude=153.03&countryCode=AU
        [HttpGet]
        public async Task<IActionResult> GetWeather(double latitude, double longitude, string? countryCode)
        {
            // Validate the input first
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                return BadRequest("Latitude must be between -90 and 90, and longitude between -180 and 180.");
            }

            try
            {
                var result = await _weatherService.GetWeatherAsync(latitude, longitude);
                result.EmergencyNumber = EmergencyNumberHelper.GetNumber(countryCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the real error for me, but show the user a friendly message
                _logger.LogError(ex, "Error getting weather for {Latitude}, {Longitude}", latitude, longitude);
                return StatusCode(502, "Sorry, the weather service is not available right now. Please try again.");
            }
        }

        // GET api/weather/search?city=Brisbane
        [HttpGet("search")]
        public async Task<IActionResult> SearchCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city) || city.Length < 2 || city.Length > 100)
            {
                return BadRequest("Please enter a city name between 2 and 100 characters.");
            }

            try
            {
                var cities = await _weatherService.SearchCityAsync(city);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for city {City}", city);
                return StatusCode(502, "Sorry, the city search is not available right now. Please try again.");
            }
        }
    }
}
