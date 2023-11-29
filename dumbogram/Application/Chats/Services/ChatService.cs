using Dumbogram.Application.Chats.Services.Errors;
using Dumbogram.Database;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Application.Chats.Services;

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

    /// <summary>
    ///     Creates chat
    /// </summary>
    /// <param name="chat"></param>
    public async Task CreateChat(Chat chat)
    {
        await _dbContext.Chats.AddAsync(chat);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Updates chat
    /// </summary>
    /// <param name="chat"></param>
    public async Task UpdateChat(Chat chat)
    {
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EnsureChatDeleted(Chat chat)
    {
        _dbContext.Chats.Remove(chat);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Reads all chats with Public visibility
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Chat>> ReadAllPublicChats()
    {
        var query = _dbContext
            .Chats
            .Where(chat => chat.ChatVisibility == ChatVisibility.Public);

        return await query.ToListAsync();
    }

    /// <summary>
    ///     Reads public or accessible (by given user) chat
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="userProfile"></param>
    /// <returns></returns>
    public async Task<Chat?> ReadPublicChatByChatId(Guid chatId)
    {
        var query = _dbContext
            .Chats
            .Where(chat => chat.Id == chatId)
            .Where(chat => chat.ChatVisibility == ChatVisibility.Public);

        return await query.SingleAsync();
    }

    public async Task<Result<Chat>> RequestPublicChatByChatId(Guid chatId)
    {
        var chat = await ReadPublicChatByChatId(chatId);
        if (chat == null)
        {
            return Result.Fail(new ChatNotFoundError());
        }

        return chat;
    }

    /// <summary>
    ///     Reads all chats with Public visibility or that are accessible by given user.
    ///     Can be used for chat searching.
    /// </summary>
    /// <param name="userProfile"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Chat>> ReadAllPublicOrAccessibleChats(UserProfile userProfile)
    {
        var query = _dbContext
            .Chats
            .Where(chat => chat.ChatVisibility == ChatVisibility.Public ||
                           chat.OwnerProfile == userProfile ||
                           chat.Memberships.Any(
                               membership =>
                                   membership.MemberId == userProfile.UserId &&
                                   membership.MembershipStatus == MembershipStatus.Joined
                           )
            );

        return await query.ToListAsync();
    }

    /// <summary>
    ///     Reads public or accessible (by given user) chat
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="userProfile"></param>
    /// <returns></returns>
    public async Task<Chat?> ReadPublicOrAccessibleChatByChatId(Guid chatId, UserProfile userProfile)
    {
        var query = _dbContext
            .Chats
            .Where(chat => chat.Id == chatId)
            .Where(chat =>
                chat.ChatVisibility == ChatVisibility.Public ||
                chat.OwnerProfile == userProfile ||
                chat.Memberships.Any(
                    membership =>
                        membership.MemberId == userProfile.UserId &&
                        membership.MembershipStatus == MembershipStatus.Joined
                )
            );

        return await query.SingleAsync();
    }

    public async Task<Result<Chat>> RequestPublicOrAccessibleChatByChatId(Guid chatId, UserProfile userProfile)
    {
        var chat = await ReadPublicOrAccessibleChatByChatId(chatId, userProfile);
        if (chat == null)
        {
            return Result.Fail(new ChatNotFoundError());
        }

        return chat;
    }

    /// <summary>
    ///     Reads only all chats are owned by given user.
    /// </summary>
    /// <param name="userProfile"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Chat>> ReadAllChatsOwnedBy(UserProfile userProfile)
    {
        var query = _dbContext
            .Chats
            .Where(chat =>
                chat.OwnerProfile == userProfile
            );

        return await query.ToListAsync();
    }

    /// <summary>
    ///     Reads chat owned by given user.
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="userProfile"></param>
    /// <returns></returns>
    public async Task<Chat?> ReadChatOwnedBy(Guid chatId, UserProfile userProfile)
    {
        var query = _dbContext
            .Chats
            .Where(chat => chat.Id == chatId)
            .Where(chat => chat.OwnerProfile == userProfile);

        return await query.SingleAsync();
    }

    public async Task<Result<Chat>> RequestChatOwnedBy(Guid chatId, UserProfile userProfile)
    {
        var chat = await ReadChatOwnedBy(chatId, userProfile);
        if (chat == null)
        {
            return Result.Fail(new ChatNotFoundError());
        }

        return chat;
    }

    /// <summary>
    ///     Reads all chats joined by given user.
    /// </summary>
    /// <param name="userProfile"></param>
    /// <returns></returns>
    public async Task<IEnumerable<Chat>> ReadAllChatsJoinedBy(UserProfile userProfile)
    {
        var query = _dbContext
            .Chats
            .Where(chat => chat.Memberships.Any(
                    membership =>
                        membership.MemberProfile == userProfile &&
                        membership.MembershipStatus == MembershipStatus.Joined
                )
            );

        return await query.ToListAsync();
    }

    /// <summary>
    ///     Reads chat joined by given user.
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="userProfile"></param>
    /// <returns></returns>
    public async Task<Chat?> ReadChatJoinedBy(Guid chatId, UserProfile userProfile)
    {
        var query = _dbContext
            .Chats
            .Where(chat => chat.Id == chatId)
            .Where(chat => chat.Memberships.Any(
                    membership =>
                        membership.MemberProfile == userProfile &&
                        membership.MembershipStatus == MembershipStatus.Joined
                )
            );

        return await query.SingleAsync();
    }

    public async Task<Result<Chat>> RequestChatJoinedBy(Guid chatId, UserProfile userProfile)
    {
        var chat = await ReadChatJoinedBy(chatId, userProfile);
        if (chat == null)
        {
            return Result.Fail(new ChatNotFoundError());
        }

        return chat;
    }
}