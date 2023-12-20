using Dumbogram.Api.Models.Files;

namespace Dumbogram.Api.Application.Files.Controllers.Dto;

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