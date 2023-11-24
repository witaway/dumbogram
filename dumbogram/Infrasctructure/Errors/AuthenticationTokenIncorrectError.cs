namespace Dumbogram.Common.Errors;

public class AuthenticationTokenIncorrectError : ApplicationError
{
    public AuthenticationTokenIncorrectError()
        : base(nameof(AuthenticationTokenIncorrectError))
    {
    }
}