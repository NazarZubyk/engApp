namespace Back.Infrastructure.Web;

public static class WebApplicationExtensions
{
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        return app;
    }
}
