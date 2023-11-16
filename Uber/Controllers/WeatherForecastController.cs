using Microsoft.AspNetCore.Mvc;

namespace Uber.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Simple API call
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "MerrNjoftimin")]
    public string Get()
    {
        return "O edon aliu o vakaf.";
    }
}