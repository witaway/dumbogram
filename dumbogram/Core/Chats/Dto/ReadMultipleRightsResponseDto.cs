using Dumbogram.Core.Chats.Models;

namespace Dumbogram.Core.Chats.Dto;

public class ReadMultipleRightsResponseDto : List<string>
{
    public ReadMultipleRightsResponseDto(IEnumerable<MembershipRight> rights)
    {
        AddRange(rights.Select(right => right.ToString()));
    }
}