# Conventions

## Key files

- `back/Program.cs` — feature registration chain
- `back/Features/WeatherForecast/` — reference feature layout

## Patterns

**Namespaces:** `Back.Features.{Name}`, `Back.Infrastructure.{Area}`

**Feature folder layout:**
```
back/Features/{Name}/
├── DependencyInjection.cs      # Add{Name}Feature()
├── {Name}Controller.cs
├── I{Name}Service.cs
├── {Name}Service.cs
└── Models/{Name}Dto.cs
```

**DI registration** (`DependencyInjection.cs`):
```csharp
services.AddScoped<I{Name}Service, {Name}Service>();
```

**Program.cs chain:**
```csharp
builder.Services
    .AddInfrastructure(builder.Configuration)
    .Add{Name}Feature();
```

**API controllers:**
- `[Route("api/[controller]")]` → e.g. `GET /api/weatherforecast`
- Inject service via constructor
- Return `ActionResult<T>` with `Ok(...)`

**DTOs:** C# `record` types in `Models/`

## Do / Don't

- Do: one feature = one folder under `Features/`
- Do: keep infrastructure code out of feature folders
- Don't: register services directly in `Program.cs` (use feature `DependencyInjection.cs`)
- Don't: commit unless the user asks
