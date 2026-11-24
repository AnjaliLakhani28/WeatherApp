using WeatherApp.Helpers;

namespace WeatherApp.Tests
{
    public class EmergencyNumberHelperTests
    {
        [Theory]
        [InlineData("AU", "000")]
        [InlineData("au", "000")]
        [InlineData("NZ", "111")]
        [InlineData("US", "911")]
        [InlineData("GB", "999")]
        public void GetNumber_KnownCountry_ReturnsLocalNumber(string countryCode, string expected)
        {
            Assert.Equal(expected, EmergencyNumberHelper.GetNumber(countryCode));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("XX")]
        public void GetNumber_UnknownCountry_Returns112(string? countryCode)
        {
            Assert.Equal("112", EmergencyNumberHelper.GetNumber(countryCode));
        }
    }
}
