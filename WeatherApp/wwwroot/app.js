// Default city when the page first opens
let selectedCity = {
    name: "Brisbane",
    country_code: "AU",
    latitude: -27.47,
    longitude: 153.03
};

// Get the weather from my C# API and show it on the page
async function loadWeather(city) {
    selectedCity = city;
    showMessage("Loading weather...");

    try {
        const url = `/api/weather?latitude=${city.latitude}&longitude=${city.longitude}&countryCode=${city.country_code || ""}`;
        const response = await fetch(url);

        if (!response.ok) {
            const errorText = await response.text();
            showMessage(errorText);
            return;
        }

        const weather = await response.json();
        showWeather(city.name, weather);
        showMessage("");
    } catch (error) {
        console.error(error);
        showMessage("Could not connect to the server. Please try again.");
    }
}

function formatHour(timeText) {
    const hour = Number(timeText.substring(11, 13));
    if (hour === 0) return "12am";
    if (hour === 12) return "12pm";
    return hour < 12 ? `${hour}am` : `${hour - 12}pm`;
}

// Put the weather data on the page.
// I use textContent (not innerHTML) so data from the API can't run as code.
function showWeather(cityName, weather) {
    document.getElementById("cityName").textContent = cityName;
    document.getElementById("currentIcon").textContent = weather.current.icon;
    document.getElementById("temperature").textContent = weather.current.temperature;
    document.getElementById("description").textContent = weather.current.description;
    document.getElementById("humidity").textContent = weather.current.humidity;
    document.getElementById("wind").textContent = weather.current.windSpeed;
    document.getElementById("tip").textContent = weather.tip;
    document.getElementById("emergency").textContent = weather.emergencyNumber;
    document.getElementById("feelsLike").textContent = weather.current.feelsLike;

    const forecastList = document.getElementById("forecast");
    forecastList.innerHTML = "";   // clear the old list

    for (let i = 0; i < weather.forecast.length; i++) {
        const day = weather.forecast[i];
        const dayName = i === 0 ? "Today" : formatDate(day.date);
        const item = document.createElement("li");

        const dayText = document.createElement("span");
        dayText.className = "day";
        dayText.textContent = dayName;

        const conditionsText = document.createElement("span");
        conditionsText.className = "conditions";
        conditionsText.textContent = `${day.icon} ${day.description}`;

        const rainText = document.createElement("span");
        rainText.className = "rain";
        rainText.textContent = `💧 ${day.rainChance}%`;

        const tempText = document.createElement("span");
        tempText.className = "temps";
        tempText.textContent = `${day.minTemp}° / ${day.maxTemp}°`;

        item.append(dayText, conditionsText, rainText, tempText);
        forecastList.appendChild(item);
    }

    const hourlyList = document.getElementById("hourly");
    hourlyList.innerHTML = "";   // clear the old list

    for (let i = 0; i < weather.hourly.length; i++) {
        const hour = weather.hourly[i];
        const hourName = i === 0 ? "Now" : formatHour(hour.time);
        const item = document.createElement("li");

        const timeText = document.createElement("span");
        timeText.className = "time";
        timeText.textContent = hourName;

        const iconText = document.createElement("span");
        iconText.className = "hour-icon";
        iconText.textContent = hour.icon;

        const tempText = document.createElement("span");
        tempText.className = "hour-temp";
        tempText.textContent = `${hour.temperature}°`;

        const rainText = document.createElement("span");
        rainText.className = "rain";
        rainText.textContent = `💧 ${hour.rainChance}%`;

        item.append(timeText, iconText, tempText, rainText);
        hourlyList.appendChild(item);
    }

    document.getElementById("weather").classList.remove("hidden");
}

// Search for cities and show the results as a list to click on
async function searchCity() {
    const cityName = document.getElementById("cityInput").value.trim();
    const resultsList = document.getElementById("cityResults");
    resultsList.innerHTML = "";

    if (cityName.length < 2) {
        showMessage("Please enter at least 2 letters.");
        return;
    }

    try {
        const response = await fetch(`/api/weather/search?city=${encodeURIComponent(cityName)}`);

        if (!response.ok) {
            showMessage(await response.text());
            return;
        }

        const cities = await response.json();

        if (cities.length === 0) {
            showMessage("No cities found. Please check the spelling.");
            return;
        }

        showMessage("");
        for (const city of cities) {
            const item = document.createElement("li");
            item.textContent = [city.name, city.admin1, city.country].filter(x => x).join(", ");
            item.addEventListener("click", () => {
                resultsList.innerHTML = "";
                document.getElementById("cityInput").value = "";
                loadWeather(city);
            });
            resultsList.appendChild(item);
        }
    } catch (error) {
        showMessage("Could not connect to the server. Please try again.");
    }
}

// Turns "2026-11-23" into "Mon 23 Nov"
function formatDate(dateText) {
    const date = new Date(dateText + "T00:00:00");
    return date.toLocaleDateString("en-AU", { weekday: "short", day: "numeric", month: "short" });
}

function showMessage(text) {
    document.getElementById("message").textContent = text;
}

// Set up the search button and Enter key, then load the default city
document.getElementById("searchButton").addEventListener("click", searchCity);
document.getElementById("cityInput").addEventListener("keydown", (event) => {
    if (event.key === "Enter") {
        searchCity();
    }
});

loadWeather(selectedCity);
