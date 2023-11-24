using System.Net;
using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Users.Errors;

public class UserNotFoundError : ApplicationApiError
{
    public UserNotFoundError()
        : base(nameof(UserNotFoundError), HttpStatusCode.NotFound)
    {
    }
}

public class UnauthorizedError : ApplicationApiError
{
    public UnauthorizedError()
        : base(nameof(UnauthorizedError), HttpStatusCode.Unauthorized)
    {
    }
}