using Dumbogram.Application.Users.Models;

namespace Dumbogram.Application.Chats.Dto;

public class ReadMultipleMembersShortInfoResponseDto : List<ReadSingleMemberShortInfoResponseDto>
{
    public ReadMultipleMembersShortInfoResponseDto(IEnumerable<UserProfile> userProfiles)
    {
        AddRange(userProfiles.Select(
            userProfile => new ReadSingleMemberShortInfoResponseDto(userProfile)
        ));
    }
}