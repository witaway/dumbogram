using FluentResults;

namespace Dumbogram.Common.Errors;

public class ApplicationError : Error
{
    protected ApplicationError(string errorCode)
    {
        ErrorCode = errorCode;
    }

    public string ErrorCode
    {
        get => GetErrorCode();
        private set => SetErrorCode(value);
    }

    public ApplicationError WithMessage(string message)
    {
        Message = message;
        return this;
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