namespace Dumbogram.Common.Exceptions;

public class BaseApplicationException : Exception
{
    public BaseApplicationException()
    {
    }

    public BaseApplicationException(string message)
        : base(message)
    {
    }

    public BaseApplicationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}