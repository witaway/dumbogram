using Dumbogram.Api.Persistence.Context.Application.Entities.Users;

namespace Dumbogram.Api.Application.Chats.Controllers.Dto;

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