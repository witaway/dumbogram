using Dumbogram.Core.Chats.Errors;
using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Core.Chats.Services;

public class ChatMembershipService
{
    private readonly ChatVisibilityService _chatVisibilityService;
    private readonly ApplicationDbContext _dbContext;

    public ChatMembershipService(
        ApplicationDbContext dbContext,
        ChatVisibilityService chatVisibilityService
    )
    {
        _dbContext = dbContext;
        _chatVisibilityService = chatVisibilityService;
    }

    public async Task EnsureUserJoinedInChat(UserProfile userProfile, Chat chat)
    {
        var chatMembership = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Joined
        };

        _dbContext.ChatMemberships.Update(chatMembership);

        await _dbContext.SaveChangesAsync();
    }

    public async Task EnsureUserBannedInChat(UserProfile userProfile, Chat chat)
    {
        var chatMembership = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Banned
        };

        _dbContext.ChatMemberships.Update(chatMembership);

        await _dbContext.SaveChangesAsync();
    }

    public async Task EnsureUserLeavedInChat(UserProfile userProfile, Chat chat)
    {
        var chatMembership = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Leaved
        };

        _dbContext.ChatMemberships.Update(chatMembership);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserProfile>> ReadAllChatJoinedUsers(Chat chat)
    {
        var query = _dbContext
            .UserProfiles
            .Where(profile => profile.Memberships.Any(
                    membership =>
                        membership.Chat == chat &&
                        membership.MembershipStatus == MembershipStatus.Joined
                )
            );

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<UserProfile>> ReadAllChatLeftUsers(Chat chat)
    {
        var query = _dbContext
            .UserProfiles
            .Where(profile => profile.Memberships.Any(
                    membership =>
                        membership.Chat == chat &&
                        membership.MembershipStatus == MembershipStatus.Leaved
                )
            );

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<UserProfile>> ReadAllChatBannedUsers(Chat chat)
    {
        var query = _dbContext
            .UserProfiles
            .Where(profile => profile.Memberships.Any(
                    membership =>
                        membership.Chat == chat &&
                        membership.MembershipStatus == MembershipStatus.Banned
                )
            );

        return await query.ToListAsync();
    }

    public async Task<Result> JoinUserToChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserBannedInChat(userProfile, chat))
        {
            var message = $"User {userProfile.Username} is banned in chat {chat.Title}";
            return Result.Fail(message);
        }

        if (await IsUserJoinedToChat(userProfile, chat))
        {
            var message = $"User {userProfile.Username} already joined to chat {chat.Title}";
            return Result.Fail(new UserAlreadyJoinedToChat(message));
        }

        await EnsureUserJoinedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<Result> InviteUserToChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserBannedInChat(userProfile, chat))
        {
            var message = $"User {userProfile.Username} is banned in chat {chat.Title}";
            return Result.Fail(message);
        }

        if (await IsUserJoinedToChat(userProfile, chat))
        {
            var message = $"User {userProfile.Username} already joined to chat {chat.Title}";
            return Result.Fail(new UserAlreadyJoinedToChat(message));
        }

        if (await IsUserBannedInChat(userProfile, chat))
        {
            var message = $"User {userProfile.Username} already left from chat ${chat.Title}";
            return Result.Fail(new UserAlreadyBannedInChat(message));
        }

        await EnsureUserJoinedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<Result> BanUserInChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserBannedInChat(userProfile, chat))
        {
            var message = $"User {userProfile.Username} already banned in chat {chat.Title}";
            return Result.Fail(new UserAlreadyBannedInChat(message));
        }

        await EnsureUserBannedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<Result> UnbanUserInChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserBannedInChat(userProfile, chat))
        {
            var message = $"User {userProfile.Username} already banned in chat ${chat.Title}";
            return Result.Fail(new UserAlreadyBannedInChat(message));
        }

        await EnsureUserLeavedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<Result> LeaveUserFromChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserBannedInChat(userProfile, chat))
        {
            var message = $"User {userProfile.Username} already left from chat ${chat.Title}";
            return Result.Fail(new UserAlreadyBannedInChat(message));
        }

        await EnsureUserBannedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<bool> IsUserJoinedToChat(UserProfile userProfile, Chat chat)
    {
        var aliveMembershipStatus = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Joined
        };

        var isMember = await _dbContext.ChatMemberships.AnyAsync(m => m == aliveMembershipStatus);

        return isMember;
    }

    public async Task<bool> IsUserBannedInChat(UserProfile userProfile, Chat chat)
    {
        var bannedMembershipStatus = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Banned
        };

        var isBanned = await _dbContext.ChatMemberships.AnyAsync(m => m == bannedMembershipStatus);

        return isBanned;
    }

    public async Task<bool> IsUserLeavedInChat(UserProfile userProfile, Chat chat)
    {
        var leavedMembershipStatus = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Leaved
        };

        var isLeaved = await _dbContext.ChatMemberships.AnyAsync(m => m == leavedMembershipStatus);

        return isLeaved;
    }
}