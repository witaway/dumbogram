using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.Application.Files.Services.Errors;

public class FileNotExistError : ApplicationApiError
{
    public FileNotExistError()
        : base(nameof(FileNotExistError), HttpStatusCode.NotFound)
    {
    }
}