# Backend architecture

Vertical-slice API — each feature in `Features/`, shared code in `Infrastructure/`.

## Key files

- `back/Program.cs` — startup and feature registration
- `back/Features/{Name}/` — feature slices
- `back/Infrastructure/` — cross-cutting concerns

## Patterns

**Program.cs flow:**
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddWeatherForecastFeature();   // add new features here

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

await app.VerifyDatabaseConnectionAsync();

app.UseInfrastructure();
app.MapControllers();

app.Run();
```

**Adding a new feature:**
1. User attaches `agent-context/entry/backend-feature.md` — agent follows that checklist
2. Or manually: create `back/Features/{Name}/`, register in `Program.cs`
3. Agent creates `features/{kebab}.md`, `handoff/{kebab}-api.md`, updates nav/index

## Do / Don't

- Do: copy `WeatherForecast` as the reference feature
- Do: put DB and HTTP middleware in `Infrastructure/`, not in features
- Don't: put business logic in controllers — use services
