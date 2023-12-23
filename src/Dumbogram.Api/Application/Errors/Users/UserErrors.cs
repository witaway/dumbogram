using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Application.Errors.Users;

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