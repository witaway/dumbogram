using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Core.Chats.Services;

public class ChatMembershipService
{
    private readonly ApplicationDbContext _dbContext;

    public ChatMembershipService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task JoinUser(UserProfile userProfile, Chat chat)
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

    public async Task BanUser(UserProfile userProfile, Chat chat)
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

    public async Task LeaveUser(UserProfile userProfile, Chat chat)
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

    public async Task<bool> IsMember(UserProfile userProfile, Chat chat)
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

    public async Task<bool> IsBanned(UserProfile userProfile, Chat chat)
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

    public async Task<bool> IsLeaved(UserProfile userProfile, Chat chat)
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