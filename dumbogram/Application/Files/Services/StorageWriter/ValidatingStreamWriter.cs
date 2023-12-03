using Dumbogram.Application.Files.Services.Exceptions;

namespace Dumbogram.Application.Files.Services.StorageWriter;

public class ValidatingStreamWriter
{
    private long LengthLimitBytes { get; set; } = long.MaxValue;
    private List<byte[]> AllowedSignatures { get; } = new();
    private bool ShouldVerifySignature => AllowedSignatures.Count > 0;


    public async Task WriteAsync(Stream source, Stream destination, int bufferSize)
    {
        if (ShouldVerifySignature)
        {
            var maxSignatureLength = AllowedSignatures.Max(signature => signature.Length);

            if (bufferSize < maxSignatureLength)
            {
                throw new StreamWriterBufferTooSmallException();
            }
        }

        var buffer = new byte[bufferSize];
        int read;
        var readTotal = 0;

        var firstIteration = true;

        // Read the whole stream
        while ((read = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            // And write to destination.
            // Flushing will be made automatically when file buffer exceeded
            await destination.WriteAsync(buffer, 0, read);
            await destination.FlushAsync();

            // Check length limit
            readTotal += read;
            if (readTotal > LengthLimitBytes)
            {
                throw new FileTooBigException();
            }

            // For first iteration (that also contains file header), check signatures
            if (firstIteration && ShouldVerifySignature)
            {
                firstIteration = false;

                var signatureMatches = AllowedSignatures
                    .Any(signature =>
                        buffer
                            .Take(signature.Length)
                            .SequenceEqual(signature)
                    );

                if (!signatureMatches)
                {
                    throw new FileTypeIncorrectException(FileTypeIncorrectness.FileSignatureIncorrect);
                }
            }
        }
    }

    public ValidatingStreamWriter WithLengthLimit(long lengthLimitBytes)
    {
        LengthLimitBytes = lengthLimitBytes;
        return this;
    }

    public ValidatingStreamWriter WithAllowedSignatures(IEnumerable<byte[]> allowedSignatures)
    {
        AllowedSignatures.AddRange(allowedSignatures);
        return this;
    }

    public ValidatingStreamWriter WithAllowedSignature(byte[] allowedSignature)
    {
        AllowedSignatures.Add(allowedSignature);
        return this;
    }
}