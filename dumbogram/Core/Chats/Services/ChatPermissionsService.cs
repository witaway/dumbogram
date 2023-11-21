using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;

namespace Dumbogram.Core.Chats.Services;

public class ChatPermissionsService
{
    private readonly ApplicationDbContext _dbContext;

    public ChatPermissionsService(
        ApplicationDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public async Task ChangeChatOwnership(Chat chat, UserProfile userProfile)
    {
        chat.OwnerProfile = userProfile;
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EnsureUserHasPermissionInChat(Chat chat, UserProfile userProfile, MembershipRight membershipRight)
    {
        var permission = new ChatMemberPermission
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipRight = membershipRight
        };
        _dbContext.ChatMemberPermissions.Update(permission);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EnsureUserHasNotPermissionInChat(Chat chat, UserProfile userProfile,
        MembershipRight membershipRight)
    {
        var permission = new ChatMemberPermission
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipRight = membershipRight
        };
        _dbContext.ChatMemberPermissions.Remove(permission);
        await _dbContext.SaveChangesAsync();
    }
}