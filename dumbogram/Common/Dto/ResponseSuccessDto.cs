namespace Dumbogram.Common.Dto;

public class ResponseSuccessDto<T> : ResponseDto
{
    public ResponseSuccessDto(string message, T data) : base(message)
    {
        Data = data;
    }

    public T? Data { get; set; }
}