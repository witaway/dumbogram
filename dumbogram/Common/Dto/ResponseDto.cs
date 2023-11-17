namespace Dumbogram.Common.Dto;

public abstract class ResponseDto
{
    protected ResponseDto(string message)
    {
        Message = message;
    }

    public string Message { get; set; }

    public static ResponseDto Failure(string message, IEnumerable<ErrorDto> errors)
    {
        return new ResponseFailureDto(message, errors);
    }

    public static ResponseDto Failure(string message)
    {
        var errors = new List<ErrorDto>();
        return new ResponseFailureDto(message, errors);
    }

    public static ResponseSuccessDto<T> Success<T>(string message, T data)
    {
        return new ResponseSuccessDto<T>(message, data);
    }
}