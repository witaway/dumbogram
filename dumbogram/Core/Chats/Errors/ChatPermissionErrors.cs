using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class NotEnoughPermissionsError : ApplicationError
{
    public NotEnoughPermissionsError()
        : base(nameof(NotEnoughPermissionsError))
    {
    }
}

public class CannotChangeOwnerRights : ApplicationError
{
    public CannotChangeOwnerRights()
        : base(nameof(CannotChangeOwnerRights))
    {
    }
}