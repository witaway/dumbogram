namespace Dumbogram.Api.Infrastructure.Files.Exceptions;

public abstract class FileUploadException : ApplicationException
{
    protected FileUploadException()
    {
    }

    protected FileUploadException(string message)
        : base(message)
    {
    }

    protected FileUploadException(string message, Exception inner)
        : base(message, inner)
    {
    }
}