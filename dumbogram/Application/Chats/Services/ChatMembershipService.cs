using Dumbogram.Application.Chats.Services.Errors;
using Dumbogram.Database;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Application.Chats.Services;

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
            return Result.Fail(new UserBannedInChatError());
        }

        if (await IsUserJoinedToChat(userProfile, chat))
        {
            return Result.Fail(new UserAlreadyJoinedToChatError());
        }

        await EnsureUserJoinedInChat(userProfile, chat);

        return Result.Ok();
    }

    public async Task<Result> InviteUserToChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserJoinedToChat(userProfile, chat))
        {
            return Result.Fail(new UserAlreadyJoinedToChatError());
        }

        if (await IsUserBannedInChat(userProfile, chat))
        {
            return Result.Fail(new UserAlreadyBannedInChatError());
        }

        await EnsureUserJoinedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<Result> BanUserInChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserBannedInChat(userProfile, chat))
        {
            return Result.Fail(new UserAlreadyBannedInChatError());
        }

        await EnsureUserBannedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<Result> UnbanUserInChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserBannedInChat(userProfile, chat))
        {
            return Result.Fail(new UserAlreadyBannedInChatError());
        }

        await EnsureUserLeavedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<Result> LeaveUserFromChat(UserProfile userProfile, Chat chat)
    {
        if (await IsUserBannedInChat(userProfile, chat))
        {
            return Result.Fail(new UserAlreadyBannedInChatError());
        }

        await EnsureUserBannedInChat(userProfile, chat);
        return Result.Ok();
    }

    public async Task<bool> IsUserJoinedToChat(UserProfile userProfile, Chat chat)
    {
        return await _dbContext
            .ChatMemberships
            .Where(m => m.Chat == chat)
            .Where(m => m.MemberProfile == userProfile)
            .Where(m => m.MembershipStatus == MembershipStatus.Joined)
            .AnyAsync();
    }

    public async Task<bool> IsUserBannedInChat(UserProfile userProfile, Chat chat)
    {
        return await _dbContext
            .ChatMemberships
            .Where(m => m.Chat == chat)
            .Where(m => m.MemberProfile == userProfile)
            .Where(m => m.MembershipStatus == MembershipStatus.Banned)
            .AnyAsync();
    }

    public async Task<bool> IsUserLeavedInChat(UserProfile userProfile, Chat chat)
    {
        return await _dbContext
            .ChatMemberships
            .Where(m => m.Chat == chat)
            .Where(m => m.MemberProfile == userProfile)
            .Where(m => m.MembershipStatus == MembershipStatus.Leaved)
            .AnyAsync();
    }
}