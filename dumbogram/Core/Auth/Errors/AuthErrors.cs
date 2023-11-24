using Dumbogram.Common.Errors;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Core.Auth.Errors;

public class PasswordNotValidError : ApplicationError
{
    public PasswordNotValidError()
        : base("PasswordNotValid")
    {
    }
}

public abstract class CredentialsConflictError : ApplicationError
{
    protected CredentialsConflictError(string errorCode)
        : base(errorCode)
    {
    }
}

public class UsernameAlreadyTakenError : ApplicationError
{
    public UsernameAlreadyTakenError()
        : base("UsernameAlreadyTaken")
    {
    }
}

public class EmailAlreadyTakenError : ApplicationError
{
    public EmailAlreadyTakenError()
        : base("EmailAlreadyTaken")
    {
    }
}

public class WrappedIdentityError : ApplicationError
{
    public WrappedIdentityError(IdentityError error)
        : base(error.Code, error.Description)
    {
    }
}