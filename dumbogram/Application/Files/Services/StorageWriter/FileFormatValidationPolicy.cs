namespace Dumbogram.Application.Files.Services.StorageWriter;

public enum FileFormatValidationPolicy
{
    /// <summary>
    ///     Any file format
    /// </summary>
    DoNotValidate,

    /// <summary>
    ///     File must match extension of one of given FileFormat's
    /// </summary>
    ValidateByExtensionOnly,

    /// <summary>
    ///     File must match signature of one of given FileFormat's
    /// </summary>
    ValidateBySignatureOnly,

    /// <summary>
    ///     File extension must match one of given file format and signature must match one of given file format
    ///     These file formats can be different
    ///     Useful when file format group is given and renaming does not make change
    ///     For example, if user renamed png to jpg
    /// </summary>
    ValidateByExtensionAndSignature,

    /// <summary>
    ///     File extension and signature must strictly match same file format
    /// </summary>
    ValidateByExtensionAndSignatureStrict
}