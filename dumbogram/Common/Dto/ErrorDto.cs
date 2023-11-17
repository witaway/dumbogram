using Dumbogram.Common.Errors;
using FluentResults;

namespace Dumbogram.Common.Dto;

public class ErrorDto
{
    public ErrorDto(string message, string code)
    {
        Message = message;
        Code = code;
    }

    public ErrorDto(BaseApplicationError error)
    {
        Message = error.Message;
        Code = error.ErrorCode;
    }

    public ErrorDto(IError error)
    {
        Message = error.Message;
        Code = "UnknownError";
    }

    public string Message { get; set; }

    public string Code { get; set; }
}