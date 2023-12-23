using Dumbogram.Api.Common.Extensions;
using Dumbogram.Api.Common.Utilities;
using Dumbogram.Api.Infrastructure.Files.StorageWriter;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Dumbogram.Api.Infrastructure.Files;

public static class GetFileContainersExtension
{
    public static IAsyncEnumerable<FileContainer> GetFileContainersFromForm(this HttpRequest request)
    {
        var formFiles = request.Form.Files;

        var fileContainers = formFiles.GetFileContainers();

        return fileContainers;
    }

    public static IAsyncEnumerable<FileContainer> GetFileContainersFromMultipart(this HttpRequest request)
    {
        var contentType = request.ContentType ?? "";

        if (!MultipartRequestHelper.IsMultipartContentType(contentType))
            throw new Exception("The request couldn't be processed");

        var boundary = MultipartRequestHelper.GetBoundary(
            MediaTypeHeaderValue.Parse(contentType),
            new FormOptions().MultipartBoundaryLengthLimit
        );

        var multipartReader = new MultipartReader(boundary, request.Body);

        var fileContainers = multipartReader.GetFileContainers();

        return fileContainers;
    }
}