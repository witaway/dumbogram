using Dumbogram.Api.Models.Chats;

namespace Dumbogram.Api.Application.Chats.Controllers.Dto;

public class ReadMultipleRightsResponse : List<string>
{
    public ReadMultipleRightsResponse(IEnumerable<MembershipRight> rights)
    {
        AddRange(rights.Select(right => right.ToString()));
    }
}