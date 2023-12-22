using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Api.ApiOld.Auth.Services.Errors;

public class PasswordNotValidError : ApplicationApiError
{
    public PasswordNotValidError()
        : base(nameof(PasswordNotValidError), HttpStatusCode.Unauthorized)
    {
    }
}

public abstract class CredentialsConflictError : ApplicationApiError
{
    protected CredentialsConflictError(string errorCode)
        : base(errorCode, HttpStatusCode.Conflict)
    {
    }
}

public class UsernameAlreadyTakenError : CredentialsConflictError
{
    public UsernameAlreadyTakenError()
        : base(nameof(UsernameAlreadyTakenError))
    {
    }
}

public class EmailAlreadyTakenError : CredentialsConflictError
{
    public EmailAlreadyTakenError()
        : base(nameof(EmailAlreadyTakenError))
    {
    }
}

public class WrappedIdentityError : ApplicationApiError
{
    public WrappedIdentityError(IdentityError error)
        : base(error.Code, HttpStatusCode.BadRequest)
    {
        WithMessage(error.Description);
    }
}