namespace Dumbogram.Common.Errors;

public class AuthenticationTokenIncorrectError : BaseApplicationError
{
    public AuthenticationTokenIncorrectError(string message)
        : base(nameof(AuthenticationTokenIncorrectError), message)
    {
    }
}