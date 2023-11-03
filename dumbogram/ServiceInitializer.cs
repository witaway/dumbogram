using Dumbogram.Dto;
using Dumbogram.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram;

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

        RegisterFluentValidation(services);
        RegisterSwagger(services);

        return services;
    }

    private static void RegisterCustomDependencies(IServiceCollection services)
    {
        // Place where custom services must register in IoC
    }

    private static void RegisterFluentValidation(IServiceCollection services)
    {
        // This is how single validator registers:
        //   services.AddScoped<IValidator<SignInDto>, SignInDtoValidator>();
        // Todo: Maybe use assembly name instead of marker validator?
        services.AddValidatorsFromAssemblyContaining(typeof(SignInDtoValidator));
        services.AddFluentValidationAutoValidation();
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