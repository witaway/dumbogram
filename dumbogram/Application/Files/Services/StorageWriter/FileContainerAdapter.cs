using Microsoft.AspNetCore.WebUtilities;

namespace Dumbogram.Application.Files.Services.StorageWriter;

public class FileContainerAdapter
{
    public FileContainerAdapter(IFormFile formFile)
    {
        Filename = formFile.FileName;
        FileMetadata = new FileMetadata(Filename)
        {
            AnnouncedLength = formFile.Length
        };

        Stream = formFile.OpenReadStream();
    }

    public FileContainerAdapter(FileMultipartSection fileMultipartSection)
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