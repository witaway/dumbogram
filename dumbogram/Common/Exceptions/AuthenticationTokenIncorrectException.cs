namespace Dumbogram.Common.Exceptions;

public class AuthenticationTokenIncorrectException : BaseApplicationException
{
    public AuthenticationTokenIncorrectException()
    {
    }

    public AuthenticationTokenIncorrectException(string message)
        : base(message)
    {
    }

    public AuthenticationTokenIncorrectException(string message, Exception inner)
        : base(message, inner)
    {
    }
}