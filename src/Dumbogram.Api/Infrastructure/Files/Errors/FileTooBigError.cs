using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Infrastructure.Files.Errors;

public class FileTooBigError : ApplicationApiError
{
    public FileTooBigError()
        : base(nameof(FileTooBigError), HttpStatusCode.BadRequest)
    {
    }
}