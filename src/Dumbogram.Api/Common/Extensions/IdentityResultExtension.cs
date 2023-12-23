using Dumbogram.Api.Application.Errors.Auth;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Api.Common.Extensions;

public static class IdentityResultExtension
{
    public static Result GetWrappedResult(this IdentityResult identityResult)
    {
        if (identityResult.Succeeded)
        {
            return Result.Ok();
        }

        var errors = identityResult.GetWrappedErrors();
        return Result.Fail(errors);
    }

    public static IEnumerable<WrappedIdentityError> GetWrappedErrors(this IdentityResult identityResult)
    {
        return identityResult.Errors
            .Select(identityError => new WrappedIdentityError(identityError))
            .ToList();
    }
}