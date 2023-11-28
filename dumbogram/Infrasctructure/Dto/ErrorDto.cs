using Dumbogram.Infrasctructure.Errors;
using FluentResults;

namespace Dumbogram.Infrasctructure.Dto;

public class ErrorDto
{
    public ErrorDto(string code)
    {
        Code = code;
    }

    public string Code { get; set; }

    public static ErrorDto FromError(IError error)
    {
        if (error is ApplicationError applicationError)
        {
            return FromApplicationError(applicationError);
        }

        return FromGenericError(error);
    }

    private static ErrorDto FromApplicationError(ApplicationError error)
    {
        return error.Message is not null
            ? new ErrorDtoWithMessage(error.ErrorCode, error.Message)
            : new ErrorDto(error.ErrorCode);
    }

    private static ErrorDto FromGenericError(IError error)
    {
        const string unknownErrorCode = "UnknownError";
        return error.Message is not null
            ? new ErrorDtoWithMessage(unknownErrorCode, error.Message)
            : new ErrorDto(unknownErrorCode);
    }
}

public class ErrorDtoWithMessage : ErrorDto
{
    public ErrorDtoWithMessage(string code, string message)
        : base(code)
    {
        Message = message;
    }

    public string? Message { get; set; }
}