using WeatherApp.Helpers;

namespace WeatherApp.Tests
{
    public class WeatherCodeHelperTests
    {
        [Theory]
        [InlineData(0, "Clear sky")]
        [InlineData(3, "Overcast")]
        [InlineData(63, "Rain")]
        [InlineData(95, "Thunderstorm")]
        public void GetDescription_KnownCode_ReturnsCorrectText(int code, string expected)
        {
            string result = WeatherCodeHelper.GetDescription(code);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetDescription_UnknownCode_ReturnsUnknown()
        {
            string result = WeatherCodeHelper.GetDescription(1000);

            Assert.Equal("Unknown", result);
        }

        [Fact]
        public void GetIcon_ClearSkyAtNight_ReturnsMoon()
        {
            string result = WeatherCodeHelper.GetIcon(0, false);

            Assert.Equal("🌙", result);
        }

        [Fact]
        public void GetIcon_ClearSkyDuringDay_ReturnsSun()
        {
            string result = WeatherCodeHelper.GetIcon(0, true);

            Assert.Equal("☀️", result);
        }
    }
}
