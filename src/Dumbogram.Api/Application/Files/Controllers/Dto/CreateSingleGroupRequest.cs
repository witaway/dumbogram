using Dumbogram.Api.Models.Files;

namespace Dumbogram.Api.Application.Files.Controllers.Dto;

public class CreateSingleGroupRequest
{
    public CreateSingleGroupRequest(FilesGroup group, FilesUploadResponse? uploadResult)
    {
        Group = new GetSingleGroupRequest(group);
        UploadResult = uploadResult;
    }

    public GetSingleGroupRequest Group { get; set; }
    public FilesUploadResponse? UploadResult { get; set; }
}