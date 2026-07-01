using Npgsql;

namespace Back.Infrastructure.Persistence;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddSingleton(NpgsqlDataSource.Create(connectionString));
        return services;
    }

    public static async Task VerifyDatabaseConnectionAsync(this WebApplication app)
    {
        await using var connection = await app.Services
            .GetRequiredService<NpgsqlDataSource>()
            .OpenConnectionAsync();

        app.Logger.LogInformation("Database connection established.");
    }
}
