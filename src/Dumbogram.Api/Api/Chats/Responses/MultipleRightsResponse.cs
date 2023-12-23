using Dumbogram.Api.Persistence.Context.Application.Enumerations;

namespace Dumbogram.Api.Api.Chats.Responses;

public class MultipleRightsResponse : List<string>
{
    public MultipleRightsResponse(IEnumerable<MembershipRight> rights)
    {
        AddRange(rights.Select(right => right.ToString()));
    }
}