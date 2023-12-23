namespace Dumbogram.Api.Infrastructure.Files.FileFormats;

public static class FileFormatExtensions
{
    private static readonly Dictionary<FileFormat, List<string>> _extensions = new()
    {
        { FileFormat.Jpeg, new List<string> { ".jpeg" } },
        { FileFormat.Jpeg2000, new List<string> { ".jpeg2000", "jp2" } },
        { FileFormat.Jpg, new List<string> { ".jpg" } },
        { FileFormat.Png, new List<string> { ".png" } },
        { FileFormat.Gif, new List<string> { ".gif" } },
        { FileFormat.Zip, new List<string> { ".zip" } },
        { FileFormat.Pdf, new List<string> { ".pdf" } },
        { FileFormat.Z, new List<string> { ".z" } },
        { FileFormat.Tar, new List<string> { ".tar" } },
        { FileFormat.TarZ, new List<string> { ".tar.z" } },
        { FileFormat.Tiff, new List<string> { ".tif", ".tiff" } },
        { FileFormat.Rar, new List<string> { ".rar" } },
        { FileFormat._7Z, new List<string> { ".7z" } },
        { FileFormat.Txt, new List<string> { ".txt " } }
    };

    public static List<string> GetExtensions(FileFormat fileFormat)
    {
        return _extensions.TryGetValue(fileFormat, out var extensions)
            ? extensions
            : new List<string>();
    }

    public static List<string> GetExtensions(IEnumerable<FileFormat> fileFormats)
    {
        var extensions = new List<string>();
        foreach (var fileFormat in fileFormats)
        {
            extensions.AddRange(GetExtensions(fileFormat));
        }

        return extensions;
    }

    public static FileFormat GetFileFormat(string extension)
    {
        return _extensions
            .Where(x => x.Value.Contains(extension))
            .Select(x => x.Key)
            .FirstOrDefault(FileFormat.Unknown);
    }
}