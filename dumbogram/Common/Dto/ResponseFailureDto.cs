namespace Dumbogram.Common.Dto;

public class ResponseFailureDto : ResponseDto
{
    public ResponseFailureDto(string message, IEnumerable<ErrorDto> errors) : base(message)
    {
        Errors = errors;
    }

    public IEnumerable<ErrorDto> Errors { get; set; }
}