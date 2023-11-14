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

    public void ChangeChatOwnership(Chat chat, UserProfile userProfile)
    {
        chat.OwnerProfile = userProfile;
        _dbContext.Chats.Update(chat);
        _dbContext.SaveChanges();
    }
}