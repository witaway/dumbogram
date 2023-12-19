using Microsoft.AspNetCore.WebUtilities;

namespace Dumbogram.Api.Application.Files.Services.StorageWriter;

public class FileContainer
{
    public FileContainer(IFormFile formFile)
    {
        Filename = formFile.FileName;
        FileMetadata = new FileMetadata(Filename)
        {
            AnnouncedLength = formFile.Length
        };

        Stream = formFile.OpenReadStream();
    }

    public FileContainer(FileMultipartSection fileMultipartSection)
    {
        Filename = fileMultipartSection.FileName;
        FileMetadata = new FileMetadata(Filename)
        {
            AnnouncedLength = fileMultipartSection.FileStream!.Length
        };

        Stream = fileMultipartSection.FileStream!;
    }

    public FileMetadata FileMetadata { get; set; }
    public string Filename { get; set; }
    public Stream Stream { get; set; }
}