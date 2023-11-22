using Dumbogram.Common.Dto;
using Dumbogram.Common.Errors;
using FluentResults;

namespace Dumbogram.Common.Extensions;

public static class ResultDtoExtensions
{
    public static Response ToFailureDto(this Result result, string message)
    {
        if (result.IsSuccess)
        {
            throw new Exception("ToFailureDto cannot be called when IsSuccess=true");
        }

        var errors = result.Errors;
        var errorDtos = errors.Select(error =>
        {
            var applicationError = error as BaseApplicationError;
            if (applicationError != null)
            {
                return new ErrorDto(applicationError);
            }

            return new ErrorDto(error);
        });

        return Response.Failure(message, errorDtos);
    }

    public static Response ToFailureDto<T>(this Result<T> result, string message)
    {
        if (result.IsSuccess)
        {
            throw new Exception("ToFailureDto cannot be called when IsSuccess=true");
        }

        var errors = result.Errors;
        var errorDtos = errors.Select(error =>
        {
            var applicationError = error as BaseApplicationError;
            if (applicationError != null)
            {
                return new ErrorDto(applicationError);
            }

            return new ErrorDto(error);
        });

        return Response.Failure(message, errorDtos);
    }
}