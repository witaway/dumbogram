using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;

namespace Dumbogram.Core.Chats.Services;

public class ChatMembershipService
{
    private readonly ApplicationDbContext _dbContext;

    public ChatMembershipService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void JoinUser(UserProfile userProfile, Chat chat)
    {
        var chatMembership = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Joined
        };
        _dbContext.ChatMemberships.Update(chatMembership);
        _dbContext.SaveChanges();
    }

    public void BanUser(UserProfile userProfile, Chat chat)
    {
        var chatMembership = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Banned
        };
        _dbContext.ChatMemberships.Update(chatMembership);
        _dbContext.SaveChanges();
    }

    public void LeaveUser(UserProfile userProfile, Chat chat)
    {
        var chatMembership = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Leaved
        };
        _dbContext.ChatMemberships.Update(chatMembership);
        _dbContext.SaveChanges();
    }

    public bool IsMember(UserProfile userProfile, Chat chat)
    {
        var aliveMembershipStatus = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Joined
        };
        var isMember = _dbContext.ChatMemberships.Any(m => m == aliveMembershipStatus);
        return isMember;
    }

    public bool IsBanned(UserProfile userProfile, Chat chat)
    {
        var bannedMembershipStatus = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Banned
        };
        var isBanned = _dbContext.ChatMemberships.Any(m => m == bannedMembershipStatus);
        return isBanned;
    }

    public bool IsLeaved(UserProfile userProfile, Chat chat)
    {
        var leavedMembershipStatus = new ChatMembership
        {
            Chat = chat,
            MemberProfile = userProfile,
            MembershipStatus = MembershipStatus.Leaved
        };
        var isLeaved = _dbContext.ChatMemberships.Any(m => m == leavedMembershipStatus);
        return isLeaved;
    }
}