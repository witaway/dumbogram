using System.Text;
using Dumbogram.Application.Auth.Controllers.Dto;
using Dumbogram.Application.Auth.Services;
using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Messages.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Database;
using Dumbogram.Database.Identity;
using Dumbogram.Infrasctructure.Filters;
using Dumbogram.Infrasctructure.Middlewares;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        services.AddHttpContextAccessor();
        RegisterCustomMiddlewares(services);
        RegisterCustomServices(services);

        services.AddControllers(ConfigureMvc);

        RegisterFluentValidation(services);
        RegisterSwagger(services);

        return services;
    }

    private static void ConfigureMvc(MvcOptions options)
    {
        options.Filters.Add<ResultFilter>();
    }

    private static void RegisterCustomMiddlewares(IServiceCollection services)
    {
        services.AddTransient<ErrorHandlerMiddleware>();
    }

    private static void RegisterCustomServices(IServiceCollection services)
    {
        // Auth-related services
        services.AddScoped<AuthService>();
        services.AddScoped<TokenService>();

        // IdentityUser-related services
        services.AddScoped<IdentityRolesService>();
        services.AddScoped<IdentityUserService>();

        // User-related services
        services.AddScoped<UserService>();

        // User resolver
        // Needed to obtain current logged in user access in other services
        services.AddScoped<UserResolverService>();

        // Chat-related services
        services.AddScoped<ChatService>();
        services.AddScoped<ChatPermissionsService>();
        services.AddScoped<ChatMembershipService>();
        services.AddScoped<ChatVisibilityService>();

        // Messages-related services
        services.AddScoped<MessagesService>();
        services.AddScoped<SystemMessagesService>();
        services.AddScoped<MessageActionsGuardService>();
    }

    private static void RegisterIdentity(IServiceCollection services)
    {
        services.AddIdentity<ApplicationIdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
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
        services.AddValidatorsFromAssemblyContaining(typeof(SignInRequestValidator));
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
        services.AddDbContext<ApplicationIdentityDbContext>(
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