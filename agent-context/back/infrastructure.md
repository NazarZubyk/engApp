# Backend infrastructure

Cross-cutting setup — database (Npgsql) and HTTP pipeline.

## Key files

- `back/Infrastructure/DependencyInjection.cs` — `AddInfrastructure()`
- `back/Infrastructure/Persistence/DatabaseExtensions.cs` — Npgsql setup
- `back/Infrastructure/Web/WebApplicationExtensions.cs` — `UseInfrastructure()`

## Patterns

**Service registration** (`Infrastructure/DependencyInjection.cs`):
```csharp
public static IServiceCollection AddInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration)
{
    services.AddDatabase(configuration);
    return services;
}
```

**Database** — connection string key `DefaultConnection` in `appsettings.Development.json`. `NpgsqlDataSource` registered as singleton. Connection verified at startup via `VerifyDatabaseConnectionAsync()`.

**HTTP pipeline** (`UseInfrastructure()`):
- Development: maps OpenAPI
- Always: HTTPS redirection

No EF Core yet — raw Npgsql only.

## Do / Don't

- Do: add new infrastructure concerns as extension methods in `Infrastructure/`
- Do: read connection string from `IConfiguration`, not hardcoded
- Don't: put feature-specific logic in `Infrastructure/`
