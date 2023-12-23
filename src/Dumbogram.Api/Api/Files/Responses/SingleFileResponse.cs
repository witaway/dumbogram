using Dumbogram.Api.Persistence.Context.Application.Entities.Files;

namespace Dumbogram.Api.Api.Files.Responses;

public class SingleFileResponse
{
    public SingleFileResponse(FileRecord fileRecord)
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