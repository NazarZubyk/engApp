# WeatherForecast

Sample API feature — copy this structure when adding new features.

## Key files

- `back/Features/WeatherForecast/WeatherForecastController.cs`
- `back/Features/WeatherForecast/IWeatherForecastService.cs`
- `back/Features/WeatherForecast/WeatherForecastService.cs`
- `back/Features/WeatherForecast/Models/WeatherForecastDto.cs`
- `back/Features/WeatherForecast/DependencyInjection.cs`

## Endpoints

| Method | Path | Returns |
|--------|------|---------|
| GET | `/api/weatherforecast` | `IEnumerable<WeatherForecastDto>` |

Test: `back/back.http` → `GET http://localhost:5286/api/weatherforecast`

## Patterns

**Controller** — inject `IWeatherForecastService`, return `Ok(_forecastService.GetForecast())`.

**DTO** — record with computed property:
```csharp
public record WeatherForecastDto(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
```

**Registration** — `AddWeatherForecastFeature()` in `Program.cs`.

## Do / Don't

- Do: copy this folder structure for new features
- Do: create `agent-context/features/{name}.md` when adding a feature
- Don't: use this as production business logic — it is sample data only
