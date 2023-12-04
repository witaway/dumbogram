using Dumbogram.Application.Files.Services.Exceptions;
using Dumbogram.Application.Files.Services.FileFormats;

namespace Dumbogram.Application.Files.Services.StorageWriter;

public class StorageWriter
{
    private readonly List<FileFormat> _permittedFileFormats = new();

    private FileFormatValidationPolicy _fileFormatMatchPolicy =
        FileFormatValidationPolicy.DoNotValidate;

    private long LengthLimitBytes { get; set; } = long.MaxValue;

    private bool ShouldValidateExtension =>
        _fileFormatMatchPolicy != FileFormatValidationPolicy.ValidateBySignatureOnly &&
        _fileFormatMatchPolicy != FileFormatValidationPolicy.DoNotValidate;

    private bool ShouldValidateSignature =>
        _fileFormatMatchPolicy != FileFormatValidationPolicy.ValidateByExtensionOnly &&
        _fileFormatMatchPolicy != FileFormatValidationPolicy.DoNotValidate;

    private void ValidateAnnouncedLength(long? length)
    {
        if (length != null && length > LengthLimitBytes)
        {
            throw new FileTooBigException();
        }
    }

    private void ValidateExtension(string extension)
    {
        var permittedExtensions = FileFormatExtensions.GetExtensions(_permittedFileFormats);
        if (ShouldValidateExtension && !permittedExtensions.Contains(extension))
        {
            throw new FileTypeIncorrectException(FileTypeIncorrectness.FileExtensionIncorrect);
        }
    }

    private void ConfigureWriterLengthValidation(ValidatingStreamWriter writer)
    {
        writer.WithLengthLimit(LengthLimitBytes);
    }

    private void ConfigureWriterSignatureValidation(ValidatingStreamWriter writer, FileFormat fileFormat)
    {
        if (ShouldValidateSignature)
        {
            var signatures = _fileFormatMatchPolicy ==
                             FileFormatValidationPolicy.ValidateByExtensionAndSignatureStrict
                ? FileFormatSignatures.GetSignatures(fileFormat)
                : FileFormatSignatures.GetSignatures(_permittedFileFormats);

            writer.WithAllowedSignatures(signatures);
        }
    }

    private async Task Write(
        FileMetadata fileMetadata,
        Stream source,
        Stream destination
    )
    {
        ValidateAnnouncedLength(fileMetadata.AnnouncedLength);
        ValidateExtension(fileMetadata.Extension);

        var writer = new ValidatingStreamWriter();
        ConfigureWriterLengthValidation(writer);
        ConfigureWriterSignatureValidation(writer, fileMetadata.FileFormat);

        try
        {
            await writer.WriteAsync(source, destination, 1024);
        }
        catch (FileTypeIncorrectException fileSignatureIncorrectException)
        {
            throw ShouldValidateExtension
                ? new FileTypeIncorrectException(FileTypeIncorrectness.FileExtensionDoesNotMatchSignature)
                : new FileTypeIncorrectException(FileTypeIncorrectness.FileSignatureIncorrect);
        }
    }

    public async Task Write(FileContainer fileContainer, Stream destination)
    {
        var fileMetadata = fileContainer.FileMetadata;
        var fileStream = fileContainer.Stream;

        await Write(fileMetadata, fileStream, destination);
    }

    public StorageWriter SetFileFormatValidationPolicy(FileFormatValidationPolicy fileFormatValidationPolicy)
    {
        _fileFormatMatchPolicy = fileFormatValidationPolicy;
        return this;
    }

    public StorageWriter SetFileLengthLimit(long bytes)
    {
        LengthLimitBytes = bytes;
        return this;
    }

    public StorageWriter AddPermittedFileFormats(IEnumerable<FileFormat> fileFormats)
    {
        _permittedFileFormats.AddRange(fileFormats);
        return this;
    }

    public StorageWriter AddPermittedFileFormat(FileFormat fileFormat)
    {
        _permittedFileFormats.Add(fileFormat);
        return this;
    }
}