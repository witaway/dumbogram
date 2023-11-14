using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;

namespace Dumbogram.Core.Chats.Services;

public class ChatService
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ApplicationDbContext _dbContext;

    public ChatService(
        ApplicationDbContext dbContext,
        ChatMembershipService chatMembershipService
    )
    {
        _dbContext = dbContext;
        _chatMembershipService = chatMembershipService;
    }

    public IEnumerable<Chat> ReadAllChats()
    {
        return _dbContext.Chats.ToList();
    }

    public IEnumerable<Chat> ReadAllChatsOwnedBy(Guid ownerId)
    {
        return _dbContext.Chats
            .Where(c => c.OwnerId == ownerId)
            .ToList();
    }

    public IEnumerable<Chat> ReadAllChatsOwnedBy(UserProfile userProfile)
    {
        return ReadAllChatsOwnedBy(userProfile.UserId);
    }

    public IEnumerable<Chat> ReadAllChatsJoinedBy(Guid memberId)
    {
        return _dbContext
            .Chats
            .Where(c => c.Memberships.Any(m => m.MemberId == memberId && m.MembershipStatus == MembershipStatus.Joined))
            .ToList();
    }

    public IEnumerable<Chat> ReadAllChatsJoinedBy(UserProfile userProfile)
    {
        return ReadAllChatsJoinedBy(userProfile.UserId);
    }

    public Chat ReadChatById(Guid chatId)
    {
        return _dbContext.Chats.Single(c => c.Id == chatId);
    }

    public void CreateChat(Chat chat)
    {
        _dbContext.Chats.Add(chat);
        _dbContext.SaveChanges();
    }

    public void UpdateChat(Chat chat)
    {
        _dbContext.Chats.Update(chat);
        _dbContext.SaveChanges();
    }
}