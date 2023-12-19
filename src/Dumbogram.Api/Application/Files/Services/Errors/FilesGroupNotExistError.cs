using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.Application.Files.Services.Errors;

public class FilesGroupNotExistError : ApplicationApiError
{
    public FilesGroupNotExistError()
        : base(nameof(FilesGroupNotExistError), HttpStatusCode.NotFound)
    {
    }
}