using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Files.Services.Errors;

public class FilesGroupNotExistError : ApplicationApiError
{
    public FilesGroupNotExistError()
        : base(nameof(FilesGroupNotExistError), HttpStatusCode.NotFound)
    {
    }
}