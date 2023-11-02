using System.Runtime.CompilerServices;

namespace dumbogram;

public static partial class ServiceInitializer
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        RegisterCustomDependencies(services);
        services.AddControllers();
        RegisterSwagger(services);
        return services;
    }

    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        // Place where custom services must register in IoC
    }

    private static void RegisterSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}