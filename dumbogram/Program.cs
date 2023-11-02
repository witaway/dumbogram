using dumbogram;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.RegisterApplicationServices(builder.Configuration);

        var app = builder.Build();

        app.ConfigureMiddleware();
        app.RegisterEndpoints();

        app.Run();
    }
}