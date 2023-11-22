using Dumbogram.Core.Users.Models;

namespace Dumbogram.Core.Chats.Dto;

public class ReadMultipleMembersShortInfoResponseDto : List<ReadSingleMemberShortInfoResponseDto>
{
    public ReadMultipleMembersShortInfoResponseDto(IEnumerable<UserProfile> userProfiles)
    {
        AddRange(userProfiles.Select(
            userProfile => new ReadSingleMemberShortInfoResponseDto(userProfile)
        ));
    }
}