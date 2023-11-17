using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;

namespace Dumbogram.Core.Chats.Services;

public class ChatPermissionsService
{
    private readonly ApplicationDbContext _dbContext;

    public ChatPermissionsService(
        ApplicationDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public async Task ChangeChatOwnership(Chat chat, UserProfile userProfile)
    {
        chat.OwnerProfile = userProfile;
        _dbContext.Chats.Update(chat);
        await _dbContext.SaveChangesAsync();
    }
}