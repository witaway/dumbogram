using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Files.Services.Errors;

public class FileNotExistError : ApplicationApiError
{
    public FileNotExistError()
        : base(nameof(FileNotExistError), HttpStatusCode.NotFound)
    {
    }
}