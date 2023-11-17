using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<Chat>> ReadAllChats()
    {
        return await _dbContext
            .Chats
            .ToListAsync();
    }

    public async Task<IEnumerable<Chat>> ReadAllChatsOwnedBy(Guid ownerId)
    {
        return await _dbContext
            .Chats
            .Where(c => c.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Chat>> ReadAllChatsOwnedBy(UserProfile userProfile)
    {
        return await ReadAllChatsOwnedBy(userProfile.UserId);
    }

    public async Task<IEnumerable<Chat>> ReadAllChatsJoinedBy(Guid memberId)
    {
        return await _dbContext
            .Chats
            .Where(c => c.Memberships.Any(m => m.MemberId == memberId && m.MembershipStatus == MembershipStatus.Joined))
            .ToListAsync();
    }

    public async Task<IEnumerable<Chat>> ReadAllChatsJoinedBy(UserProfile userProfile)
    {
        return await ReadAllChatsJoinedBy(userProfile.UserId);
    }

    public async Task<Chat?> ReadChatById(Guid chatId)
    {
        return await _dbContext
            .Chats
            .SingleAsync(c => c.Id == chatId);
    }

    public async Task CreateChat(Chat chat)
    {
        await _dbContext.Chats.AddAsync(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateChat(Chat chat)
    {
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync();
    }
}