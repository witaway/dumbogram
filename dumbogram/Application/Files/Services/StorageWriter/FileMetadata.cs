using System.Net;
using Dumbogram.Application.Files.Services.FileFormats;
using Dumbogram.Infrasctructure.Utilities;

namespace Dumbogram.Application.Files.Services.StorageWriter;

public class FileMetadata
{
    public FileMetadata(string originalFilename)
    {
        OriginalFileName = Path.GetFileName(originalFilename);

        TrustedFileNameForDisplay = SanitizeFileName(originalFilename);

        Extension = Path.GetExtension(OriginalFileName).ToLowerInvariant();

        FileFormat = FileFormatExtensions.GetFileFormat(Extension);

        MimeType = FileFormatMimeTypes.GetMimeTypeOrDefault(FileFormat);
    }

    public long? AnnouncedLength { get; set; }
    public string OriginalFileName { get; set; }
    public string TrustedFileNameForDisplay { get; set; }
    public string MimeType { get; set; }
    public string Extension { get; set; }
    public FileFormat FileFormat { get; set; }

    private static string SanitizeFileName(string fileName)
    {
        // Make sure user doesn't sent path to another directory. Just in case.
        var extractedFileName = Path.GetFileName(fileName);
        // Sanitize filename from special characters and words
        var sanitizedFileName = PathUtility.SanitizeFileName(extractedFileName);
        // Prevent XSS attacks
        var htmlEncodedFileName = WebUtility.HtmlEncode(sanitizedFileName);

        return htmlEncodedFileName;
    }
}