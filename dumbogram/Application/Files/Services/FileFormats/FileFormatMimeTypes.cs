using Microsoft.AspNetCore.StaticFiles;

namespace Dumbogram.Application.Files.Services.FileFormats;

public static class FileFormatMimeTypes
{
    private static readonly FileExtensionContentTypeProvider _provider = new();

    public static string? GetMimeType(FileFormat fileFormat)
    {
        var extensions = FileFormatExtensions.GetExtensions(fileFormat);
        foreach (var extension in extensions)
        {
            if (_provider.TryGetContentType(extension, out var mimeType))
            {
                return mimeType;
            }
        }

        return null;
    }

    public static string GetMimeTypeOrDefault(FileFormat fileFormat)
    {
        var extensions = FileFormatExtensions.GetExtensions(fileFormat);
        foreach (var extension in extensions)
        {
            if (_provider.TryGetContentType(extension, out var mimeType))
            {
                return mimeType;
            }
        }

        return "application/octet-stream";
    }
}