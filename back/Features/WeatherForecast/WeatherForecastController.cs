using Back.Features.WeatherForecast.Models;
using Microsoft.AspNetCore.Mvc;

namespace Back.Features.WeatherForecast;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _forecastService;

    public WeatherForecastController(IWeatherForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<WeatherForecastDto>> Get()
    {
        return Ok(_forecastService.GetForecast());
    }
}
