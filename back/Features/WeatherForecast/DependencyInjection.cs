namespace Back.Features.WeatherForecast;

public static class DependencyInjection
{
    public static IServiceCollection AddWeatherForecastFeature(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        return services;
    }
}
