using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.ApiOld.Files.Services.Errors;

public class FileTypeIncorrectError : ApplicationApiError
{
    public FileTypeIncorrectError()
        : base(nameof(FileTypeIncorrectError), HttpStatusCode.BadRequest)
    {
    }
}