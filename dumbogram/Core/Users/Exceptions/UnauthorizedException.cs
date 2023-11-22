using Dumbogram.Common.Exceptions;

namespace Dumbogram.Core.Users.Exceptions;

public class UnauthorizedException : BaseApplicationException
{
    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception inner) : base(message, inner)
    {
    }
}