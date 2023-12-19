namespace Dumbogram.Api.Application.Files.Services.FileFormats;

public static class FileFormatGroups
{
    public static readonly List<FileFormat> Photo = new()
    {
        FileFormat.Jpeg,
        FileFormat.Png,
        FileFormat.Jpeg2000,
        FileFormat.Jpg
    };

    public static readonly List<FileFormat> Audio = new()
    {
        FileFormat.Mp3
    };
}