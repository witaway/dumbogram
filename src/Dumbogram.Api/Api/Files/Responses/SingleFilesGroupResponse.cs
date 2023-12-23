using Dumbogram.Api.Persistence.Context.Application.Entities.Files;

namespace Dumbogram.Api.Api.Files.Responses;

public class SingleFilesGroupResponse
{
    public SingleFilesGroupResponse(FilesGroup filesGroup)
    {
        Id = filesGroup.Id;
        Type = filesGroup.GroupType.ToString();
        Files = filesGroup.Files.Select(file => new SingleFileResponse(file)).ToList();
    }

    public List<SingleFileResponse> Files { get; set; }
    public Guid Id { get; set; }
    public string Type { get; set; }
}