namespace Dumbogram.Application.Files.Services.Exceptions;

public class StreamWriterBufferTooSmallException : ApplicationException
{
    public StreamWriterBufferTooSmallException()
    {
    }

    public StreamWriterBufferTooSmallException(string message)
        : base(message)
    {
    }

    public StreamWriterBufferTooSmallException(string message, Exception inner)
        : base(message, inner)
    {
    }
}