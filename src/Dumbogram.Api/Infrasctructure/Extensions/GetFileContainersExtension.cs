using Dumbogram.Api.Application.Files.Services.StorageWriter;
using Microsoft.AspNetCore.WebUtilities;

namespace Dumbogram.Api.Infrasctructure.Extensions;

public static class GetFileContainersExtension
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

    public static async IAsyncEnumerable<FileContainer> GetFileContainers(
        this MultipartReader multipartReader
    )
    {
        await foreach (var fileMultipartSection in multipartReader.GetFileMultipartSections())
        {
            var fileContainer = new FileContainer(fileMultipartSection);
            yield return fileContainer;
        }
    }

    public static async IAsyncEnumerable<FileContainer> GetFileContainers(
        this IFormFileCollection formFiles
    )
    {
        foreach (var formFile in formFiles)
        {
            var fileContainer = new FileContainer(formFile);
            yield return fileContainer;
        }
    }
}