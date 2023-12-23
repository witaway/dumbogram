using Dumbogram.Api.Common.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dumbogram.Api.Common.Controller;

[DefaultStatusCode(DefaultStatusCode)]
public class FailureObjectResult : ObjectResult
{
    private const int DefaultStatusCode = StatusCodes.Status400BadRequest;

    public FailureObjectResult(IEnumerable<IError> errors)
        : base(errors)
    {
        var statusCode = DetermineStatusCode(errors) ?? DefaultStatusCode;
        StatusCode = statusCode;
    }

    public FailureObjectResult(IError error)
        : this(new List<IError> { error })
    {
    }

    private static int? DetermineStatusCode(IEnumerable<IError> errors)
    {
        var error = errors.FirstOrDefault(
            error => error is ApplicationApiError,
            null
        );

        if (error is ApplicationApiError apiError)
        {
            var statusCode = (int)apiError.StatusCode;
            return statusCode;
        }

        return null;
    }
}