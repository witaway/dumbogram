using FluentResults;

namespace Dumbogram.Common.Errors;

public class BaseApplicationError : Error
{
    public BaseApplicationError(string errorCode, string message)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public BaseApplicationError(string errorCode, string message, BaseApplicationError causedBy)
        : base(message, causedBy)
    {
        ErrorCode = errorCode;
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
        if (Metadata.TryGetValue("ErrorCode", out var errorCode))
        {
            return errorCode as string;
        }

        return "";
    }
}