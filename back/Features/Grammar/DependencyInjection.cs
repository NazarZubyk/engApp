namespace Back.Features.Grammar;

public static class DependencyInjection
{
    public static IServiceCollection AddGrammarFeature(this IServiceCollection services)
    {
        services.AddScoped<IGrammarContentService, GrammarContentService>();
        return services;
    }
}
