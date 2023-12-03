namespace Dumbogram.Application.Files.Services.FileFormats;

public static class FileFormatSignatures
{
    private static readonly Dictionary<FileFormat, List<byte[]>> _fileSignatures = new()
    {
        #region Big list of file extension signatures

        {
            FileFormat.Gif, new List<byte[]>
            {
                new byte[] { 0x47, 0x49, 0x46, 0x38 }
            }
        },
        {
            FileFormat.Png, new List<byte[]>
            {
                new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
            }
        },
        {
            FileFormat.Jpeg, new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }
            }
        },
        {
            FileFormat.Jpeg2000, new List<byte[]>
            {
                new byte[] { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20, 0x0D, 0x0A, 0x87, 0x0A }
            }
        },
        {
            FileFormat.Jpg, new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }
            }
        },
        {
            FileFormat.Zip, new List<byte[]> //also docx, xlsx, pptx, ...
            {
                new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 }
            }
        },
        {
            FileFormat.Pdf, new List<byte[]>
            {
                new byte[] { 0x25, 0x50, 0x44, 0x46 }
            }
        },
        {
            FileFormat.Z, new List<byte[]>
            {
                new byte[] { 0x1F, 0x9D },
                new byte[] { 0x1F, 0xA0 }
            }
        },
        {
            FileFormat.Tar, new List<byte[]>
            {
                new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72, 0x00, 0x30, 0x30 },
                new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72, 0x20, 0x20, 0x00 }
            }
        },
        {
            FileFormat.TarZ, new List<byte[]>
            {
                new byte[] { 0x1F, 0x9D },
                new byte[] { 0x1F, 0xA0 }
            }
        },
        {
            FileFormat.Tiff, new List<byte[]>
            {
                new byte[] { 0x49, 0x49, 0x2A, 0x00 },
                new byte[] { 0x4D, 0x4D, 0x00, 0x2A }
            }
        },
        {
            FileFormat.Rar, new List<byte[]>
            {
                new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 },
                new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00 }
            }
        },
        {
            FileFormat._7Z, new List<byte[]>
            {
                new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }
            }
        },
        {
            FileFormat.Txt, new List<byte[]>
            {
                new byte[] { 0xEF, 0xBB, 0xBF },
                new byte[] { 0xFF, 0xFE },
                new byte[] { 0xFE, 0xFF },
                new byte[] { 0x00, 0x00, 0xFE, 0xFF }
            }
        },
        {
            FileFormat.Mp3, new List<byte[]>
            {
                new byte[] { 0xFF, 0xFB },
                new byte[] { 0xFF, 0xF3 },
                new byte[] { 0xFF, 0xF2 },
                new byte[] { 0x49, 0x44, 0x43 }
            }
        }

        #endregion
    };

    public static List<byte[]> GetSignatures(FileFormat fileFormat)
    {
        return _fileSignatures.TryGetValue(fileFormat, out var signatures)
            ? signatures
            : new List<byte[]>();
    }

    public static List<byte[]> GetSignatures(IEnumerable<FileFormat> fileFormats)
    {
        var signatures = new List<byte[]>();
        foreach (var fileFormat in fileFormats)
        {
            signatures.AddRange(GetSignatures(fileFormat));
        }

        return signatures;
    }
}