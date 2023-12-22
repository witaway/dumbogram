using Dumbogram.Api.Persistence.Context.Application.Entities.Files;

namespace Dumbogram.Api.ApiOld.Files.Controllers.Dto;

public class GetSingleFileRequest
{
    public GetSingleFileRequest(FileRecord fileRecord)
    {
        Id = fileRecord.Id;
        FileName = fileRecord.OriginalFileName;
        FileSize = fileRecord.FileSize;
        MimeType = fileRecord.MimeType;
    }

    public Guid Id { get; set; }
    public string? FileName { get; set; }
    public long FileSize { get; set; }
    public string MimeType { get; set; }
}