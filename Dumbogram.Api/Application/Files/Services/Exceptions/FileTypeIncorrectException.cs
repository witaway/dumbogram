namespace Dumbogram.Api.Application.Files.Services.Exceptions;

public enum FileTypeIncorrectness
{
    FileExtensionIncorrect,
    FileSignatureIncorrect,
    FileExtensionDoesNotMatchSignature
}

public class FileTypeIncorrectException : FileUploadException
{
    public FileTypeIncorrectException(FileTypeIncorrectness fileTypeIncorrectness)
    {
        FileTypeIncorrectness = fileTypeIncorrectness;
    }

    public FileTypeIncorrectException(FileTypeIncorrectness fileTypeIncorrectness, string message)
        : base(message)
    {
        FileTypeIncorrectness = fileTypeIncorrectness;
    }

    public FileTypeIncorrectException(FileTypeIncorrectness fileTypeIncorrectness, string message, Exception inner)
        : base(message, inner)
    {
        FileTypeIncorrectness = fileTypeIncorrectness;
    }

    public FileTypeIncorrectness FileTypeIncorrectness { get; }
}