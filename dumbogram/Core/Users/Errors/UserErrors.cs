using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Users.Errors;

public class UserNotFoundError : BaseApplicationError
{
    public UserNotFoundError(string message)
        : base(nameof(UserNotFoundError), message)
    {
    }
}

public class UnauthorizedError : BaseApplicationError
{
    public UnauthorizedError(string message)
        : base(nameof(UnauthorizedError), message)
    {
    }
}