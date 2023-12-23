using Dumbogram.Api.Common.Classes;
using Dumbogram.Api.Common.Dto;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;

namespace Dumbogram.Api.Api.Files.Responses;

public class SingleFileUploadResultResponse
{
    public SingleFileUploadResultResponse(Results<string, FileRecord>.IdentityWithResult fileResult)
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

    // File info on upload
    public int Number { get; set; }
    public string FileName { get; set; }

    // Is uploaded successfully or not
    public bool Success { get; set; }

    // Not null if success
    public Guid? FileId { get; set; }
    public Guid? GroupId { get; set; }

    // Not null if failure
    public List<ErrorDto>? Errors { get; set; }
}