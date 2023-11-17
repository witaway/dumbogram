namespace Dumbogram.Common.Dto;

public abstract class Response
{
    protected Response(string message)
    {
        Message = message;
    }

    public string Message { get; set; }

    public static Response Failure(string message, IEnumerable<ErrorDto> errors)
    {
        return new ResponseFailure(message, errors);
    }

    public static Response Failure(string message)
    {
        var errors = new List<ErrorDto>();
        return new ResponseFailure(message, errors);
    }

    public static ResponseSuccess<T> Success<T>(string message, T data)
    {
        return new ResponseSuccess<T>(message, data);
    }

    public static ResponseSuccess Success(string message)
    {
        return new ResponseSuccess(message);
    }
}