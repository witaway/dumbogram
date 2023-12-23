using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Infrastructure.Files.Errors;

public class UploadLimitExceededError : ApplicationApiError
{
    public UploadLimitExceededError()
        : base(nameof(UploadLimitExceededError), HttpStatusCode.BadRequest)
    {
    }
}