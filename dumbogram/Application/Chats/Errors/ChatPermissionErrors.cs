using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Chats.Errors;

public class NotEnoughPermissionsError : ApplicationApiError
{
    public NotEnoughPermissionsError()
        : base(nameof(NotEnoughPermissionsError), HttpStatusCode.Forbidden)
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