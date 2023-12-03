using Dumbogram.Application.Files.Services.Exceptions;
using Dumbogram.Application.Files.Services.FileFormats;
using Microsoft.AspNetCore.WebUtilities;

namespace Dumbogram.Application.Files.Services.StorageWriter;

public class StorageWriter
{
    private readonly List<FileFormat> _permittedFileFormats = new();
    private FileFormatValidationPolicy _fileFormatMatchPolicy = FileFormatValidationPolicy.DoNotValidate;

    private long LengthLimitBytes { get; } = long.MaxValue;

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
            var signatures = _fileFormatMatchPolicy == FileFormatValidationPolicy.ValidateByExtensionAndSignatureStrict
                ? FileFormatSignatures.GetSignatures(fileFormat)
                : FileFormatSignatures.GetSignatures(_permittedFileFormats);

            writer.WithAllowedSignatures(signatures);
        }
    }

    public async Task<FileMetadata> Write(
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

        return fileMetadata;
    }

    public FileMetadata GetMetadata(IFormFile formFile)
    {
        var fileName = formFile.FileName;
        var metadata = new FileMetadata(fileName);
        metadata.AnnouncedLength = formFile.Length;
        return metadata;
    }

    public FileMetadata GetMetadata(FileMultipartSection fileMultipartSection)
    {
        var fileName = fileMultipartSection.FileName;
        var metadata = new FileMetadata(fileName);
        metadata.AnnouncedLength = fileMultipartSection.FileStream!.Length;
        return metadata;
    }

    public async Task<FileMetadata> Write(IFormFile formFile, Stream destination)
    {
        var fileMetadata = GetMetadata(formFile);
        var fileStream = formFile.OpenReadStream();

        return await Write(fileMetadata, fileStream, destination);
    }

    public async Task<FileMetadata> Write(FileMultipartSection fileMultipartSection, Stream destination)
    {
        var fileMetadata = GetMetadata(fileMultipartSection);
        var fileStream = fileMultipartSection.FileStream;

        return await Write(fileMetadata, fileStream!, destination);
    }

    public StorageWriter MatchPolicy(FileFormatValidationPolicy fileFormatValidationPolicy)
    {
        _fileFormatMatchPolicy = fileFormatValidationPolicy;
        return this;
    }

    public StorageWriter AddFileFormats(IEnumerable<FileFormat> fileFormats)
    {
        _permittedFileFormats.AddRange(fileFormats);
        return this;
    }

    public StorageWriter AddFileFormat(FileFormat fileFormat)
    {
        _permittedFileFormats.Add(fileFormat);
        return this;
    }
}