using Dumbogram.Models.Chats;

namespace Dumbogram.Application.Chats.Controllers.Dto;

public class ReadMultipleRightsResponse : List<string>
{
    public ReadMultipleRightsResponse(IEnumerable<MembershipRight> rights)
    {
        AddRange(rights.Select(right => right.ToString()));
    }
}