using Dumbogram.Common.Utilities;
using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;
using Microsoft.EntityFrameworkCore;

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

    public UserProfile GetOwner(Chat chat)
    {
        return chat.OwnerProfile;
    }

    public bool IsUserOwnerOfChat(Chat chat, UserProfile userProfile)
    {
        return chat.OwnerProfile == userProfile;
    }

    public async Task ChangeChatOwnership(Chat chat, UserProfile userProfile)
    {
        chat.OwnerProfile = userProfile;
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<MembershipRight>> GetUsersRightsInChat(Chat chat, UserProfile userProfile)
    {
        var chatMemberPermissions = await _dbContext.ChatMemberPermissions
            .Where(permission => permission.Chat == chat)
            .Where(permission => permission.MemberProfile == userProfile)
            .ToListAsync();

        var rights = chatMemberPermissions.Select(permission => permission.MembershipRight).ToList();

        // TODO: Maybe remove OWNER right and use IsUserOwnedOfChat function instead?
        if (rights.Contains(MembershipRight.Owner))
        {
            var allMembershipRights = EnumUtility.GetValues<MembershipRight>();
            return allMembershipRights;
        }

        return rights;
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