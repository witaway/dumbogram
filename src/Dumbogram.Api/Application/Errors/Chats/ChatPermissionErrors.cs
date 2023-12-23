using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Application.Errors.Chats;

public class NotEnoughRightsError : ApplicationApiError
{
    public NotEnoughRightsError()
        : base(nameof(NotEnoughRightsError), HttpStatusCode.Forbidden)
    {
    }
}

public class CannotChangeOwnerRights : ApplicationApiError
{
    public CannotChangeOwnerRights()
        : base(nameof(CannotChangeOwnerRights), HttpStatusCode.Forbidden)
    {
    }
}