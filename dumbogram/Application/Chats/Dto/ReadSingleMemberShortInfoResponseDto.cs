using Dumbogram.Models.Users;

namespace Dumbogram.Application.Chats.Dto;

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