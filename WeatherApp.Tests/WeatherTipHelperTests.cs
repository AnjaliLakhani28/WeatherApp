using WeatherApp.Helpers;
using WeatherApp.Models;

namespace WeatherApp.Tests
{
    public class WeatherTipHelperTests
    {
        [Fact]
        public void GetTip_UvExactly8_ReturnsSunscreenTip()
        {
            // Arrange
            var today = new DayForecast { UvIndex = 8, RainChance = 0, MaxTemp = 25 };

            // Act
            string tip = WeatherTipHelper.GetTip(today);

            // Assert
            Assert.Contains("sunscreen", tip);
        }

        [Fact]
        public void GetTip_UvJustBelow8_DoesNotReturnSunscreenTip()
        {
            var today = new DayForecast { UvIndex = 7.9, RainChance = 0, MaxTemp = 25 };

            string tip = WeatherTipHelper.GetTip(today);

            Assert.DoesNotContain("sunscreen", tip);
        }

        [Fact]
        public void GetTip_RainChance60_ReturnsUmbrellaTip()
        {
            var today = new DayForecast { UvIndex = 3, RainChance = 60, MaxTemp = 25 };

            string tip = WeatherTipHelper.GetTip(today);

            Assert.Contains("umbrella", tip);
        }

        [Fact]
        public void GetTip_MildDay_ReturnsDefaultTip()
        {
            var today = new DayForecast { UvIndex = 3, RainChance = 10, MaxTemp = 25 };

            string tip = WeatherTipHelper.GetTip(today);

            Assert.Equal("Enjoy your day!", tip);
        }
    }
}
