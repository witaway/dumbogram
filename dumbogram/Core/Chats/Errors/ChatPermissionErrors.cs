﻿using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class NotEnoughPermissionsError : BaseApplicationError
{
    public NotEnoughPermissionsError(string message)
        : base(nameof(NotEnoughPermissionsError), message)
    {
    }
}

public class CannotChangeOwnerRights : BaseApplicationError
{
    public CannotChangeOwnerRights(string message)
        : base(nameof(CannotChangeOwnerRights), message)
    {
    }
}