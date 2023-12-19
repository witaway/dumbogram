using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.Application.Users.Services.Errors;

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