﻿using System.Net;
using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

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