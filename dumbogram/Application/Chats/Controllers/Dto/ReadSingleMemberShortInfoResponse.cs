using Dumbogram.Models.Users;

namespace Dumbogram.Application.Chats.Controllers.Dto;

public class ReadSingleMemberShortInfoResponse
{
    public Guid UserId;
    public string Username;

    public ReadSingleMemberShortInfoResponse(UserProfile userProfile)
    {
        UserId = userProfile.UserId;
        Username = userProfile.Username;
    }
}