using System.Text;
using Dumbogram.Core.Auth.Dto;
using Dumbogram.Database;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dumbogram;

public static class ServiceInitializer
{
    public static IServiceCollection RegisterApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        RegisterDbContext(services, configuration);
        RegisterIdentity(services);
        ConfigureIdentity(services);
        RegisterAuthentication(services, configuration);

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

    private static void RegisterIdentity(IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();
    }

    // TODO: Centralize Identity configuration???
    private static void ConfigureIdentity(IServiceCollection services)
    {
        services.Configure<IdentityOptions>(options =>
        {
            // TEMPORARY SOLUTION
            // Disabled all password requirements to simplify development
            // TODO: Maybe for Development environment disable all policies?
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        });
    }

    private static void RegisterAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });
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
                var connectionString = configuration.GetConnectionString("DumbogramApplicationDbConnection");
                options
                    .UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention();
            }
        );
        services.AddDbContext<IdentityDbContext>(
            options =>
            {
                var connectionString = configuration.GetConnectionString("DumbogramIdentityDbConnection");
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