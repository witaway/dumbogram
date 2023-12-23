using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;

namespace Dumbogram.Api.Domain.Services.Messages;

public class SystemMessagesService
{
    private readonly ApplicationDbContext _dbContext;

    public SystemMessagesService(
        ApplicationDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    private async Task EnsureSystemMessageCreated(SystemMessage systemMessage)
    {
        await _dbContext.AddAsync(systemMessage);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateChatCreatedMessage(Chat chat)
    {
        var message = new SystemMessage
        {
            Chat = chat,
            SystemMessageType = SystemMessageType.ChatCreated
        };
        await EnsureSystemMessageCreated(message);
    }

    public async Task CreateJoinedMessage(Chat chat, UserProfile joinedUser)
    {
        var message = new SystemMessage
        {
            Chat = chat,
            SenderProfile = joinedUser,
            SystemMessageType = SystemMessageType.UserJoined
        };
        await EnsureSystemMessageCreated(message);
    }

    public async Task CreateLeftMessage(Chat chat, UserProfile leftUser)
    {
        var message = new SystemMessage
        {
            Chat = chat,
            SenderProfile = leftUser,
            SystemMessageType = SystemMessageType.UserLeft
        };
        await EnsureSystemMessageCreated(message);
    }

    public async Task CreateEditedTitleMessage(Chat chat, UserProfile subject, string newTitle)
    {
        var message = new SystemMessage
        {
            Chat = chat,
            SenderProfile = subject,
            SystemMessageType = SystemMessageType.ChatTitleEdited,
            SystemMessageDetails = SystemMessageDetails.ChatTitleEdited(newTitle)
        };
        await EnsureSystemMessageCreated(message);
    }

    public async Task CreateEditedDescriptionMessage(Chat chat, UserProfile subject, string newDescription)
    {
        var message = new SystemMessage
        {
            Chat = chat,
            SenderProfile = subject,
            SystemMessageType = SystemMessageType.ChatDescriptionEdited,
            SystemMessageDetails = SystemMessageDetails.ChatDescriptionEdited(newDescription)
        };
        await EnsureSystemMessageCreated(message);
    }
}