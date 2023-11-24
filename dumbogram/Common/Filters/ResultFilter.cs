using Dumbogram.Common.Dto;
using Dumbogram.Common.Errors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dumbogram.Common.Filters;

public class ResultFilter : IAsyncResultFilter
{
    private readonly ILogger<ResultFilter> _logger;

    public ResultFilter(ILogger<ResultFilter> logger)
    {
        _logger = logger;
    }

    public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // Skip responses without value
        // Also skip responses where value is already formatted
        if (context.Result is not ObjectResult result ||
            result.Value is null ||
            result.Value is Response)
        {
            return next();
        }

        var value = result.Value!;

        // Process single error as list of multiple errors
        if (value is IError error)
        {
            value = new List<IError> { error };
        }

        // If result is a list of errors, use Response.failure format
        if (value is List<IError> errors)
        {
            var apiErrorDtos = errors
                .Where(errorObject => errorObject is ApplicationApiError)
                .Select(ErrorDto.FromError);

            var internalErrors = errors
                .Where(errorObject => errorObject is not ApplicationApiError);

            if (internalErrors.Any())
            {
                var actionId = context.ActionDescriptor.Id;
                var actionName = context.ActionDescriptor.DisplayName;

                _logger.LogWarning("Action {actionId} ({actionName}) tries to return internal errors: {internalErrors}",
                    actionId,
                    actionName,
                    internalErrors
                );
            }

            var valueFailure = Response.Failure(apiErrorDtos);
            result.Value = valueFailure;

            return next();
        }

        // If result is another type of object except IError or List<IError>, interpret it as Response.success
        var valueSuccess = Response.Success(value);
        result.Value = valueSuccess;

        return next();
    }
}