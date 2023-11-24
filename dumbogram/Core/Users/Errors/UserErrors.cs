using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Users.Errors;

public class UserNotFoundError : ApplicationError
{
    public UserNotFoundError()
        : base(nameof(UserNotFoundError))
    {
    }
}

public class UnauthorizedError : ApplicationError
{
    public UnauthorizedError()
        : base(nameof(UnauthorizedError))
    {
    }
}