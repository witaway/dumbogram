namespace Dumbogram.Infrasctructure.Dto;

public abstract class Response
{
    public static Response Failure(IEnumerable<ErrorDto> errors)
    {
        return new ResponseFailure(errors);
    }

    public static ResponseSuccess<T> Success<T>(T data)
    {
        return new ResponseSuccess<T>(data);
    }

    public static ResponseSuccess Success()
    {
        return new ResponseSuccess();
    }
}