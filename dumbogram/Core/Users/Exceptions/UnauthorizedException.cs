using ApplicationException = Dumbogram.Common.Exceptions.ApplicationException;

namespace Dumbogram.Core.Users.Exceptions;

public class UnauthorizedException : ApplicationException
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception inner) : base(message, inner)
    {
    }
}