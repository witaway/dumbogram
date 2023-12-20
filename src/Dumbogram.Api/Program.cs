using Serilog;

namespace Dumbogram.Api;

internal class Program
{
    public static int Main(string[] args)
    {
        SetupSerilog();

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureServices(builder.Configuration);
            builder.Host.UseSerilog();

            var app = builder.Build();
            app.ConfigureApplication();

            app.Run();
        }
        // Expose exceptions if it's not HostAbortedException
        // That's because there are exception that throws only when development
        // Just like HostAbortedException, showing up when "dotnet ef migrations add"
        catch (Exception exception) when (exception is not HostAbortedException)
        {
            Log.Fatal(exception, "Application terminated unexpectedly");
            return exception.HResult;
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return 0;
    }

    private static void SetupSerilog()
    {
        // Temporary IConfiguration to init serilog
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", true)
            .AddEnvironmentVariables()
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}