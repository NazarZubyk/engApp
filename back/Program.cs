using Back.Features.Auth;
using Back.Features.Grammar;
using Back.Features.WeatherForecast;
using Back.Infrastructure;
using Back.Infrastructure.Persistence;
using Back.Infrastructure.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddWeatherForecastFeature()
    .AddAuthFeature()
    .AddGrammarFeature();

builder.Services.AddControllers();

builder.Services.AddOpenApi(); 

var app = builder.Build();

await app.VerifyDatabaseConnectionAsync();

app.UseInfrastructure();
app.MapControllers();

app.Run();
