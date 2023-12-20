using Dumbogram.Api.Infrasctructure.Middlewares;

namespace Dumbogram.Api;

public static class SetupApplication
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.ConfigureApplicationFeatures();
        app.ConfigureApplicationEndpointMapping();

        return app;
    }

    private static WebApplication ConfigureApplicationFeatures(this WebApplication app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        return app;
    }

    private static WebApplication ConfigureApplicationEndpointMapping(this WebApplication app)
    {
        app.MapControllers();
        return app;
    }
}