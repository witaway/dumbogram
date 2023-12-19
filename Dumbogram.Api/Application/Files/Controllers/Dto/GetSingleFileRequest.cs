using File = Dumbogram.Api.Models.Files.File;

namespace Dumbogram.Api.Application.Files.Controllers.Dto;

public class GetSingleFileRequest
{
    public GetSingleFileRequest(File file)
    {
        Id = file.Id;
        FileName = file.OriginalFileName;
        FileSize = file.FileSize;
        MimeType = file.MimeType;
    }

    public Guid Id { get; set; }
    public string? FileName { get; set; }
    public long FileSize { get; set; }
    public string MimeType { get; set; }
}