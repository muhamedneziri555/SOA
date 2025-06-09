using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventMngt.Services;

public interface IWeatherService
{
    Task<WeatherInfo?> GetWeatherForLocationAsync(string location);
}

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["WeatherApi:ApiKey"] ?? throw new InvalidOperationException("Weather API key not configured");
        _httpClient.BaseAddress = new Uri("https://api.weatherapi.com/v1/");
        _logger = logger;
    }

    public async Task<WeatherInfo?> GetWeatherForLocationAsync(string location)
    {
        try
        {
            var response = await _httpClient.GetAsync($"current.json?key={_apiKey}&q={location}");
            response.EnsureSuccessStatusCode();
            
            var weatherData = await response.Content.ReadFromJsonAsync<WeatherApiResponse>();
            if (weatherData?.Current == null) return null;

            return new WeatherInfo
            {
                Temperature = weatherData.Current.TempC,
                Condition = weatherData.Current.Condition.Text,
                Humidity = weatherData.Current.Humidity,
                WindSpeed = weatherData.Current.WindKph,
                Location = weatherData.Location.Name
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data for location {Location}", location);
            return null;
        }
    }
}

public class WeatherInfo
{
    public double Temperature { get; set; }
    public string Condition { get; set; } = string.Empty;
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class WeatherApiResponse
{
    public Location Location { get; set; } = new();
    public Current Current { get; set; } = new();
}

public class Location
{
    public string Name { get; set; } = string.Empty;
}

public class Current
{
    public double TempC { get; set; }
    public Condition Condition { get; set; } = new();
    public int Humidity { get; set; }
    public double WindKph { get; set; }
}

public class Condition
{
    public string Text { get; set; } = string.Empty;
} 