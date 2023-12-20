using Dumbogram.Api.Persistence.Context.Application.Entities.Users;

namespace Dumbogram.Api.Application.Chats.Controllers.Dto;

public class ReadMultipleMembersShortInfoResponse : List<ReadSingleMemberShortInfoResponse>
{
    public ReadMultipleMembersShortInfoResponse(IEnumerable<UserProfile> userProfiles)
    {
        AddRange(userProfiles.Select(
            userProfile => new ReadSingleMemberShortInfoResponse(userProfile)
        ));
    }
}