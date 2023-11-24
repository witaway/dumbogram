using Dumbogram.Common.Dto;
using FluentResults;

namespace Dumbogram.Common.Extensions;

public static class ResultDtoExtensions
{
    public static Response ToFailureDto(this IResultBase result)
    {
        if (result.IsSuccess)
        {
            throw new Exception("ToFailureDto cannot be called when IsSuccess=true");
        }

        var errors = result.Errors;
        var errorDtos = errors.Select(ErrorDto.FromError);

        return Response.Failure(errorDtos);
    }
}