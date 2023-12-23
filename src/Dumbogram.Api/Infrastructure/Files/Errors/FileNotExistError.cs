using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Infrastructure.Files.Errors;

public class FileNotExistError : ApplicationApiError
{
    public FileNotExistError()
        : base(nameof(FileNotExistError), HttpStatusCode.NotFound)
    {
    }
}