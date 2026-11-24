using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AddHttpClient gives WeatherService an HttpClient and registers it for dependency injection
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

var app = builder.Build();

// Swagger lets me test the API endpoints in the browser while developing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();   // opens wwwroot/index.html by default
app.UseStaticFiles();    // serves the HTML, CSS and JavaScript files

app.MapControllers();

app.Run();
