namespace Dumbogram.Api.Application.Files.Services.Exceptions;

public class FileTooBigException : FileUploadException
{
    public FileTooBigException()
    {
    }

    public FileTooBigException(string message)
        : base(message)
    {
    }

    public FileTooBigException(string message, Exception inner)
        : base(message, inner)
    {
    }
}