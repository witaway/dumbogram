using Dumbogram.Core.Users.Models;

namespace Dumbogram.Core.Chats.Dto;

public class ReadSingleMemberShortInfoResponseDto
{
    public Guid UserId;
    public string Username;

    public ReadSingleMemberShortInfoResponseDto(UserProfile userProfile)
    {
        UserId = userProfile.UserId;
        Username = userProfile.Username;
    }
}