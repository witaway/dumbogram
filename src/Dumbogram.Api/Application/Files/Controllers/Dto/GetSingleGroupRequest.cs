using Dumbogram.Api.Persistence.Context.Application.Entities.Files;

namespace Dumbogram.Api.Application.Files.Controllers.Dto;

public class GetSingleGroupRequest
{
    public GetSingleGroupRequest(FilesGroup filesGroup)
    {
        Id = filesGroup.Id;
        Type = filesGroup.GroupType.ToString();
        Files = filesGroup.Files.Select(file => new GetSingleFileRequest(file)).ToList();
    }

    public List<GetSingleFileRequest> Files { get; set; }
    public Guid Id { get; set; }
    public string Type { get; set; }
}