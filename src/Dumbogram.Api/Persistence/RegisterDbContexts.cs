using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Dumbogram.Api.Persistence.Context.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Dumbogram.Api.Persistence;

public static class RegisterDbContexts
{
    public static IServiceCollection RegisterApplicationDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DumbogramApplicationDbConnection");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<SystemMessageType>();
        dataSourceBuilder.MapEnum<FilesGroupType>();
        dataSourceBuilder.MapEnum<FileType>();
        dataSourceBuilder.UseNodaTime();

        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options
                    .UseNpgsql(dataSource)
                    .UseSnakeCaseNamingConvention();
            }
        );

        return services;
    }

    public static IServiceCollection RegisterIdentityDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("DumbogramIdentityDbConnection");

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNodaTime();

        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationIdentityDbContext>(
            options =>
            {
                options
                    .UseNpgsql(dataSource)
                    .UseSnakeCaseNamingConvention();
            }
        );

        return services;
    }
}