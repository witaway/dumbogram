using Dumbogram.Application.Chats.Models;

namespace Dumbogram.Application.Chats.Dto;

public class ReadMultipleRightsResponseDto : List<string>
{
    public ReadMultipleRightsResponseDto(IEnumerable<MembershipRight> rights)
    {
        AddRange(rights.Select(right => right.ToString()));
    }
}