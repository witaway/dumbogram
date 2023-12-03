using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Files.Services.Errors;

public class FileTypeIncorrectError : ApplicationApiError
{
    public FileTypeIncorrectError()
        : base(nameof(FileTypeIncorrectError), HttpStatusCode.BadRequest)
    {
    }
}