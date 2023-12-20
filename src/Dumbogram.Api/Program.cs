namespace Dumbogram.Api;

internal class Program
{
    private static readonly bool IsDevelopment =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    public static int Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureServices(builder.Configuration);

            var app = builder.Build();
            app.ConfigureApplication();
            
            app.Run();
        }
        // Expose exceptions when environment is Development
        // That's because there are exception that throws only when development
        // Example is HostAbortedException, showing up when for example "dotnet ef migrations add"
        // Todo: Think about it. Maybe use: "when (exception is not HostAbortedException)" is better solution?
        catch (Exception exception) when (!IsDevelopment)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[FATAL] Error has occured during initialization");
            Console.ResetColor();
            Console.WriteLine(exception);
            return exception.HResult;
        }

        return 0;
    }
}