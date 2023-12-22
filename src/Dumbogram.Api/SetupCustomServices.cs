using Dumbogram.Api.ApiOld.Auth.Services;
using Dumbogram.Api.ApiOld.Chats.Services;
using Dumbogram.Api.ApiOld.Files.Services;
using Dumbogram.Api.ApiOld.Messages.Services;
using Dumbogram.Api.ApiOld.Users.Services;

namespace Dumbogram.Api;

public static class SetupCustomServices
{
    public static IServiceCollection RegisterCustomServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
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
        services.AddScoped<MessageContentBuilderService>();
        services.AddScoped<MessagesService>();
        services.AddScoped<SystemMessagesService>();
        services.AddScoped<MessageActionsGuardService>();

        // Files-related services
        services.AddSingleton<FileStorageService>();
        services.AddSingleton<FileTransferService>();
        services.AddScoped<UploadService>();
        services.AddScoped<FileRecordService>();
        services.AddScoped<FilesGroupService>();
        services.AddScoped<FileTransferService>();

        return services;
    }
}