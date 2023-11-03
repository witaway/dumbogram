namespace Dumbogram;

internal class Program
{
    public static int Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.RegisterApplicationServices(builder.Configuration);

            var app = builder.Build();
            
            app.ConfigureMiddleware();
            app.RegisterEndpoints();

            app.Run();
        }
        catch (Exception exception)
        {
            // TEMPORARY SOLUTION!!
            // Todo: Change WriteLine to normal logger
            // How to? Logger initializes after app.Build(),
            //    but errors might be occured later during WebApplicationBuilder configuration
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[FATAL] Error has occured during initialization");
            Console.ResetColor();
            Console.WriteLine(exception);
            return exception.HResult;
        }

        return 0;
    }
}