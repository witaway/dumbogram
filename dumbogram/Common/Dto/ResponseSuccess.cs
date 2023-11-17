namespace Dumbogram.Common.Dto;

public class ResponseSuccess<T> : Response
{
    public ResponseSuccess(string message, T data) : base(message)
    {
        Data = data;
    }

    public T? Data { get; set; }
}

public class ResponseSuccess : Response
{
    public ResponseSuccess(string message) : base(message)
    {
    }
}