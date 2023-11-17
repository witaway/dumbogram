using Dumbogram.Common.Errors;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Core.Auth.Errors;

public class PasswordNotValidError : BaseApplicationError
{
    public PasswordNotValidError(string message)
        : base("PasswordNotValid", message)
    {
    }
}

public abstract class CredentialsConflictError : BaseApplicationError
{
    protected CredentialsConflictError(string errorCode, string message)
        : base(errorCode, message)
    {
    }
}

public class UsernameAlreadyTakenError : BaseApplicationError
{
    public UsernameAlreadyTakenError(string message)
        : base("UsernameAlreadyTaken", message)
    {
    }
}

public class EmailAlreadyTakenError : BaseApplicationError
{
    public EmailAlreadyTakenError(string message)
        : base("EmailAlreadyTaken", message)
    {
    }
}

public class WrappedIdentityError : BaseApplicationError
{
    public WrappedIdentityError(IdentityError error)
        : base(error.Code, error.Description)
    {
    }
}