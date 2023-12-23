using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Infrastructure.Files.Errors;

public class FilesGroupNotExistError : ApplicationApiError
{
    public FilesGroupNotExistError()
        : base(nameof(FilesGroupNotExistError), HttpStatusCode.NotFound)
    {
    }
}