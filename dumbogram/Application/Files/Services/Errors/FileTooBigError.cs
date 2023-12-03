using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Files.Services.Errors;

public class FileTooBigError : ApplicationApiError
{
    public FileTooBigError()
        : base(nameof(FileTooBigError), HttpStatusCode.BadRequest)
    {
    }
}