using Microsoft.AspNetCore.Mvc;

namespace JF91.VaultSharp.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IConfiguration _configuration;

    public WeatherForecastController
    (
        ILogger<WeatherForecastController> logger,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var tests = _configuration.GetVaultSecrets
        (
            "kv",
            "test"
        );

        var test1 = _configuration.GetVaultSecret
        (
            "kv",
            "test",
            "test"
        );

        var test2 = _configuration.GetVaultSecret
        (
            "kv",
            "test",
            "test2"
        );

        _configuration.InjectSecretToConfiguration
        (
            "kv",
            "test",
            "test2",
            "Secrets:Test",
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        );

        return Enumerable.Range
            (
                1,
                5
            ).Select
            (
                index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next
                    (
                        -20,
                        55
                    ),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }
            )
            .ToArray();
    }
}