using System.Text;
using System.Text.Json.Serialization;
using Dumbogram.Api.Application.Auth.Controllers.Dto;
using Dumbogram.Api.Infrasctructure.Filters;
using Dumbogram.Api.Infrasctructure.Middlewares;
using Dumbogram.Api.Infrasctructure.ModelBinders;
using Dumbogram.Api.Persistence;
using Dumbogram.Api.Persistence.Context.Identity;
using Dumbogram.Api.Persistence.Context.Identity.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dumbogram.Api;

public static class SetupServices
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.RegisterApplicationDbContext(configuration);
        services.RegisterIdentityDbContext(configuration);

        RegisterIdentity(services);
        ConfigureIdentity(services);
        RegisterAuthentication(services, configuration);

        services.AddHttpContextAccessor();
        RegisterCustomMiddlewares(services);
        services.RegisterCustomServices(configuration);

        services
            .AddControllers(ConfigureMvc)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        RegisterFluentValidation(services);
        RegisterSwagger(services);

        return services;
    }

    private static void ConfigureMvc(MvcOptions options)
    {
        options.Filters.Add<ResultFilter>();
        options.ModelBinderProviders.Insert(0, new QueryBooleanModelBinderProvider());
    }

    private static void RegisterCustomMiddlewares(IServiceCollection services)
    {
        services.AddTransient<ErrorHandlerMiddleware>();
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

    private static void RegisterSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}