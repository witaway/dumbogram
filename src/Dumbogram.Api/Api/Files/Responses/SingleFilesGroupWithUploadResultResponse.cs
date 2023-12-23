using Dumbogram.Api.Persistence.Context.Application.Entities.Files;

namespace Dumbogram.Api.Api.Files.Responses;

public class SingleFilesGroupWithUploadResultResponse
{
    public SingleFilesGroupWithUploadResultResponse(FilesGroup group, MultipleFilesUploadResultsResponse? uploadResult)
    {
        Group = new SingleFilesGroupResponse(group);
        UploadResult = uploadResult;
    }

    public SingleFilesGroupResponse Group { get; set; }
    public MultipleFilesUploadResultsResponse? UploadResult { get; set; }
}