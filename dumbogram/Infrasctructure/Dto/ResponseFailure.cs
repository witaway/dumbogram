namespace Dumbogram.Infrasctructure.Dto;

public class ResponseFailure : Response
{
    public ResponseFailure(IEnumerable<ErrorDto> errors)
    {
        Errors = errors;
    }

    public IEnumerable<ErrorDto> Errors { get; set; }
}