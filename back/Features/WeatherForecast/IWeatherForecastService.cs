using Back.Features.WeatherForecast.Models;

namespace Back.Features.WeatherForecast;

public interface IWeatherForecastService
{
    IEnumerable<WeatherForecastDto> GetForecast(int days = 5);
}
