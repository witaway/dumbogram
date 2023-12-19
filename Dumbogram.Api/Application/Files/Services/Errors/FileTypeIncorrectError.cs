using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.Application.Files.Services.Errors;

public class FileTypeIncorrectError : ApplicationApiError
{
    public FileTypeIncorrectError()
        : base(nameof(FileTypeIncorrectError), HttpStatusCode.BadRequest)
    {
    }
}