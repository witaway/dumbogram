using Dumbogram.Database;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Messages.SystemMessages;
using Dumbogram.Models.Users;

namespace Dumbogram.Application.Chats.Services;

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

    public async Task CreateJoinedMessage(Chat chat, UserProfile joinedUser)
    {
        var message = new JoinedSystemMessage
        {
            Chat = chat,
            SubjectProfile = joinedUser
        };
        await EnsureSystemMessageCreated(message);
    }

    public async Task CreateLeftMessage(Chat chat, UserProfile leftUser)
    {
        var message = new LeftSystemMessage
        {
            Chat = chat,
            SubjectProfile = leftUser
        };
        await EnsureSystemMessageCreated(message);
    }

    public async Task CreateEditedTitleMessage(Chat chat, UserProfile subject, string newTitle)
    {
        var message = new EditedTitleSystemMessage
        {
            Chat = chat,
            SubjectProfile = subject,
            NewTitle = newTitle
        };
        await EnsureSystemMessageCreated(message);
    }

    public async Task CreateEditedDescriptionMessage(Chat chat, UserProfile subject, string newDescription)
    {
        var message = new EditedDescriptionSystemMessage
        {
            Chat = chat,
            SubjectProfile = subject,
            NewDescription = newDescription
        };
        await EnsureSystemMessageCreated(message);
    }
}