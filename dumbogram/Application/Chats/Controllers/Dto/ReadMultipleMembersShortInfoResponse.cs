using Dumbogram.Models.Users;

namespace Dumbogram.Application.Chats.Controllers.Dto;

public class ReadMultipleMembersShortInfoResponse : List<ReadSingleMemberShortInfoResponse>
{
    public ReadMultipleMembersShortInfoResponse(IEnumerable<UserProfile> userProfiles)
    {
        AddRange(userProfiles.Select(
            userProfile => new ReadSingleMemberShortInfoResponse(userProfile)
        ));
    }
}