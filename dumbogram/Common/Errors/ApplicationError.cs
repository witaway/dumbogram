using FluentResults;

namespace Dumbogram.Common.Errors;

public class ApplicationError : Error
{
    public ApplicationError(string errorCode)
    {
        ErrorCode = errorCode;
    }

    public ApplicationError(string errorCode, IError causedBy)
    {
        ErrorCode = errorCode;
        CausedBy(causedBy);
    }

    public ApplicationError(string errorCode, string message)
        : this(errorCode)
    {
        Message = message;
    }

    public ApplicationError(string errorCode, string message, IError causedBy)
        : this(errorCode, causedBy)
    {
        Message = message;
    }

    public string ErrorCode
    {
        get => GetErrorCode();
        private set => SetErrorCode(value);
    }

    private void SetErrorCode(string errorCode)
    {
        Metadata.Add("ErrorCode", errorCode);
    }

    private string GetErrorCode()
    {
        Metadata.TryGetValue("ErrorCode", out var errorCodeObject);

        if (errorCodeObject is string errorCode)
        {
            return errorCode;
        }

        return "";
    }
}