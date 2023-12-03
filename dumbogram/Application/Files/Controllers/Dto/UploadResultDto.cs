using Dumbogram.Infrasctructure.Classes;
using Dumbogram.Infrasctructure.Dto;
using File = Dumbogram.Models.Files.File;

namespace Dumbogram.Application.Files.Controllers.Dto;

public class UploadResultDto
{
    public UploadResultDto(Results<string, File>.IdentityWithResult fileResult)
    {
        Number = fileResult.Identity.Number;
        FileName = fileResult.Identity.Label;
        Success = fileResult.Result.IsSuccess;

        if (Success)
        {
            var file = fileResult.Result.Value;
            FileId = file.Id;
        }
        else
        {
            Errors = fileResult.Result.Errors
                .Select(ErrorDto.FromError)
                .ToList();
        }
    }

    public Guid? FileId { get; set; }
    public Guid? GroupId { get; set; }
    public int Number { get; set; }
    public string FileName { get; set; }
    public bool Success { get; set; }
    public List<ErrorDto>? Errors { get; set; }
}