namespace Dumbogram.Api.Common.Dto;

public class ResponseSuccess<T> : Response
{
    public ResponseSuccess(T data)
    {
        Data = data;
    }

    public T? Data { get; set; }
}

public class ResponseSuccess : Response
{
}