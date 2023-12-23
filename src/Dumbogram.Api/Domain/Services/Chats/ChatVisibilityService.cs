﻿using Dumbogram.Api.Application.Errors.Chats;
using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;

namespace Dumbogram.Api.Domain.Services.Chats;

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
        if (IsChatPrivate(chat)) return Result.Fail(new ChatAlreadyPrivateError());

        await EnsureChatPrivate(chat);

        return Result.Ok();
    }

    public async Task<Result> MakeChatPublic(Chat chat)
    {
        if (IsChatPublic(chat)) return Result.Fail(new ChatAlreadyPublicError());

        await EnsureChatPublic(chat);

        return Result.Ok();
    }
}