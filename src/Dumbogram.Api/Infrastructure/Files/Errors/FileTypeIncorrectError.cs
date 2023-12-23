using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Infrastructure.Files.Errors;

public class FileTypeIncorrectError : ApplicationApiError
{
    public FileTypeIncorrectError()
        : base(nameof(FileTypeIncorrectError), HttpStatusCode.BadRequest)
    {
    }
}