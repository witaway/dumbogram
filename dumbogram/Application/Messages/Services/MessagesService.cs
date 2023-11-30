using Dumbogram.Database;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Messages;
using Dumbogram.Models.Messages.UserMessages;
using Dumbogram.Models.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Application.Messages.Services;

public class MessagesService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly MessageActionsGuardService _messageActionsGuardService;

    public MessagesService(
        ApplicationDbContext dbContext,
        MessageActionsGuardService messageActionsGuardService
    )
    {
        _dbContext = dbContext;
        _messageActionsGuardService = messageActionsGuardService;
    }

    public async Task<Message> ReadSingleMessageById(Chat chat, int messageId)
    {
        var query = _dbContext
            .Messages
            .Where(message => message.Chat == chat)
            .Where(message => message.Id == messageId);

        var message = await query.SingleAsync();
        return message;
    }

    public async Task<List<Message>> ReadManyMessages(Chat chat)
    {
        var query = _dbContext
            .Messages
            .Where(message => message.Chat == chat);

        var messages = await query.ToListAsync();
        return messages;
    }

    public async Task<Result<Message>> QuerySingleMessageById(UserProfile subjectUser, Chat chat, int messageId)
    {
        var messagesCanBeReadResult = await _messageActionsGuardService.CheckMessagesCanBeRead(subjectUser, chat);
        if (messagesCanBeReadResult.IsFailed)
        {
            return Result.Fail(messagesCanBeReadResult.Errors);
        }

        var message = await ReadSingleMessageById(chat, messageId);
        return message;
    }

    public async Task<Result<List<Message>>> QueryManyMessages(UserProfile subjectUser, Chat chat)
    {
        var messagesCanBeReadResult = await _messageActionsGuardService.CheckMessagesCanBeRead(subjectUser, chat);
        if (messagesCanBeReadResult.IsFailed)
        {
            return Result.Fail(messagesCanBeReadResult.Errors);
        }

        var messages = await ReadManyMessages(chat);
        return Result.Ok(messages);
    }

    public async Task<Result> SendMessage(UserProfile subjectUser, UserMessage message)
    {
        var messageCanBeSendResult = await _messageActionsGuardService.CheckMessageCanBeSend(subjectUser, message);
        if (messageCanBeSendResult.IsFailed)
        {
            return Result.Fail(messageCanBeSendResult.Errors);
        }

        await _dbContext.AddAsync(message);
        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> UpdateMessage(UserProfile subjectUser, UserMessage message)
    {
        var messageCanBeUpdatedResult =
            await _messageActionsGuardService.CheckMessageCanBeUpdatedBy(subjectUser, message);
        if (messageCanBeUpdatedResult.IsFailed)
        {
            return Result.Fail(messageCanBeUpdatedResult.Errors);
        }

        _dbContext.Update(message);
        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteMessage(UserProfile subjectUser, Message message)
    {
        var messageCanBeDeletedResult =
            await _messageActionsGuardService.CheckMessageCanBeDeletedBy(subjectUser, message);
        if (messageCanBeDeletedResult.IsFailed)
        {
            return Result.Fail(messageCanBeDeletedResult.Errors);
        }

        _dbContext.Remove(message);
        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}