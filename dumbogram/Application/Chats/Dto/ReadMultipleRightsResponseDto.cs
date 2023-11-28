using Dumbogram.Models.Chats;

namespace Dumbogram.Application.Chats.Dto;

public class ReadMultipleRightsResponseDto : List<string>
{
    public ReadMultipleRightsResponseDto(IEnumerable<MembershipRight> rights)
    {
        AddRange(rights.Select(right => right.ToString()));
    }
}