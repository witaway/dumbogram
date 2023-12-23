using Dumbogram.Api.Persistence.Context.Application.Entities.Users;

namespace Dumbogram.Api.Api.Chats.Responses;

public class MultipleMembersShortInfoResponse : List<SingleMemberShortInfoResponse>
{
    public MultipleMembersShortInfoResponse(IEnumerable<UserProfile> userProfiles)
    {
        AddRange(userProfiles.Select(
            userProfile => new SingleMemberShortInfoResponse(userProfile)
        ));
    }
}