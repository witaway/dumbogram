using dumbogram.Models;
using Microsoft.EntityFrameworkCore;

namespace dumbogram;

public static class ServiceInitializer
{
    public static IServiceCollection RegisterApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        RegisterDbContext(services, configuration);
        RegisterCustomDependencies(services);
        services.AddControllers();
        RegisterSwagger(services);
        return services;
    }

    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        // Place where custom services must register in IoC
    }

    private static void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                var connectionString = configuration.GetConnectionString("DumbogramDbConnection");
                options
                    .UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention();
            }
        );
    }

    private static void RegisterSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}