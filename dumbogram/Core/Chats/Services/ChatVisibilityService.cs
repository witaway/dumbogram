using Dumbogram.Core.Chats.Errors;
using Dumbogram.Core.Chats.Models;
using Dumbogram.Database;
using FluentResults;

namespace Dumbogram.Core.Chats.Services;

public class ChatVisibilityService
{
    private readonly ApplicationDbContext _dbContext;

    public ChatVisibilityService(
        ApplicationDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public bool IsChatPrivate(Chat chat)
    {
        return chat.ChatVisibility == ChatVisibility.Private;
    }

    public bool IsChatPublic(Chat chat)
    {
        return chat.ChatVisibility == ChatVisibility.Public;
    }

    public async Task EnsureChatPrivate(Chat chat)
    {
        chat.ChatVisibility = ChatVisibility.Private;
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task EnsureChatPublic(Chat chat)
    {
        chat.ChatVisibility = ChatVisibility.Public;
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Result> MakeChatPrivate(Chat chat)
    {
        if (IsChatPrivate(chat))
        {
            var message = $"Chat {chat.Title} is already private";
            return Result.Fail(new ChatAlreadyPrivateError(message));
        }

        await EnsureChatPrivate(chat);

        return Result.Ok();
    }

    public async Task<Result> MakeChatPublic(Chat chat)
    {
        if (IsChatPublic(chat))
        {
            var message = $"Chat {chat.Title} is already public";
            return Result.Fail(new ChatAlreadyPublicError(message));
        }

        await EnsureChatPublic(chat);

        return Result.Ok();
    }
}