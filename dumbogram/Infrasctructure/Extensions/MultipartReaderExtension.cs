using Microsoft.AspNetCore.WebUtilities;

namespace Dumbogram.Infrasctructure.Extensions;

public static class MultipartReaderExtension
{
    public static async IAsyncEnumerable<FileMultipartSection> GetFileMultipartSections(
        this MultipartReader multipartReader
    )
    {
        MultipartSection? section;
        while ((section = await multipartReader.ReadNextSectionAsync()) != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                yield return fileSection;
            }
        }
    }
}