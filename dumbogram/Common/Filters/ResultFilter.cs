using Dumbogram.Common.Dto;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dumbogram.Common.Filters;

public class ResultFilter : IAsyncResultFilter
{
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
            var errorDtos = errors.Select(ErrorDto.FromError);
            var valueFailure = Response.Failure(errorDtos);
            result.Value = valueFailure;
            return next();
        }

        // If result is another type of object except IError or List<IError>, interpret it as Response.success
        var valueSuccess = Response.Success(value);
        result.Value = valueSuccess;

        return next();
    }
}