using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Messages;
using Dumbogram.Api.Domain.Services.Tokens;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Infrastructure.Files;

namespace Dumbogram.Api;

public static class SetupCustomServices
{
    public static IServiceCollection RegisterCustomServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Auth-related services
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

        // Files-related services
        services.AddSingleton<FileStorageService>();
        services.AddSingleton<FileTransferService>();
        services.AddScoped<FileRecordService>();
        services.AddScoped<FilesGroupService>();
        services.AddScoped<FileTransferService>();

        return services;
    }
}