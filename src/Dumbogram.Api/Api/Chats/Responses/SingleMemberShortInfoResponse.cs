using Dumbogram.Api.Persistence.Context.Application.Entities.Users;

namespace Dumbogram.Api.Api.Chats.Responses;

public class SingleMemberShortInfoResponse
{
    public Guid UserId;
    public string Username;

    public SingleMemberShortInfoResponse(UserProfile userProfile)
    {
        UserId = userProfile.UserId;
        Username = userProfile.Username;
    }
}