using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.Application.Files.Services.Errors;

public class FileTooBigError : ApplicationApiError
{
    public FileTooBigError()
        : base(nameof(FileTooBigError), HttpStatusCode.BadRequest)
    {
    }
}