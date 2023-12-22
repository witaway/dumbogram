using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.ApiOld.Files.Services.Errors;

public class FileTooBigError : ApplicationApiError
{
    public FileTooBigError()
        : base(nameof(FileTooBigError), HttpStatusCode.BadRequest)
    {
    }
}