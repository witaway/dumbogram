using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.Application.Chats.Services.Errors;

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