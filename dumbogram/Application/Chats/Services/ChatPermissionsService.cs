using Dumbogram.Database;
using Dumbogram.Infrasctructure.Utilities;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Application.Chats.Services;

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

    public async Task<IEnumerable<MembershipRight>> ReadAllRightsAppliedToUsersInChat(Chat chat,
        UserProfile userProfile)
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

    public async Task<bool> IsUserHasRightInChat(Chat chat, UserProfile userProfile, MembershipRight right)
    {
        return await _dbContext
            .ChatMemberPermissions
            .AnyAsync(permission =>
                permission.Chat == chat &&
                permission.MemberProfile == userProfile &&
                permission.MembershipRight == right
            );
    }

    public async Task EnsureRightsAppliedToUserInChat(Chat chat, UserProfile userProfile,
        List<MembershipRight> newRights)
    {
        var oldRights = (await ReadAllRightsAppliedToUsersInChat(chat, userProfile)).ToList();

        var removedRights = oldRights.Except(newRights).ToList();
        await EnsureUserHasNotPermissionsInChat(chat, userProfile, removedRights);

        var addedRights = newRights.Except(oldRights).ToList();
        await EnsureUserHasPermissionsInChat(chat, userProfile, addedRights);
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

    public async Task EnsureUserHasPermissionsInChat(
        Chat chat,
        UserProfile userProfile,
        IEnumerable<MembershipRight> membershipRights)
    {
        foreach (var right in membershipRights)
        {
            await EnsureUserHasPermissionInChat(chat, userProfile, right);
        }

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

    public async Task EnsureUserHasNotPermissionsInChat(
        Chat chat,
        UserProfile userProfile,
        IEnumerable<MembershipRight> membershipRights)
    {
        foreach (var right in membershipRights)
        {
            await EnsureUserHasNotPermissionInChat(chat, userProfile, right);
        }

        await _dbContext.SaveChangesAsync();
    }
}