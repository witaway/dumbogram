namespace Dumbogram.Common.Dto;

public class ResponseFailure : Response
{
    public ResponseFailure(string message, IEnumerable<ErrorDto> errors) : base(message)
    {
        Errors = errors;
    }

    public IEnumerable<ErrorDto> Errors { get; set; }
}