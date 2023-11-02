namespace dumbogram;

public static partial class EndpointsMapper
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}