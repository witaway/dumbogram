using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.ApiOld.Files.Services.Errors;

public class UploadLimitExceededError : ApplicationApiError
{
    public UploadLimitExceededError()
        : base(nameof(UploadLimitExceededError), HttpStatusCode.BadRequest)
    {
    }
}