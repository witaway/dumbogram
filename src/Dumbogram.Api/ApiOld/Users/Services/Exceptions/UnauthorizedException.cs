namespace Dumbogram.Api.ApiOld.Users.Services.Exceptions;

public class UnauthorizedException : Infrasctructure.Exceptions.ApplicationException
{
    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string message) : base(message)
    {
    }

    public UnauthorizedException(string message, Exception inner) : base(message, inner)
    {
    }
}