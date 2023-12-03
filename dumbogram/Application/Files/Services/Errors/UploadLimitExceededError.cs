using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Files.Services.Errors;

public class UploadLimitExceededError : ApplicationApiError
{
    public UploadLimitExceededError()
        : base(nameof(FileNotExistError), HttpStatusCode.BadRequest)
    {
    }
}