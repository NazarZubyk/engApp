using Back.Infrastructure.Auth;
using Back.Infrastructure.Persistence;

namespace Back.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddJwtAuthentication(configuration);
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}
