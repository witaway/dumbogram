namespace Dumbogram;

public static class EndpointsMapper
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}