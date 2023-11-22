using System.Security.Claims;
using System.Security.Principal;
using Dumbogram.Common.Errors;
using Dumbogram.Common.Exceptions;
using FluentResults;

namespace Dumbogram.Common.Extensions;

public static class PrincipalExtension
{
    public static Result<string> TryGetIdentityUserId(this IPrincipal principal)
    {
        if (principal.Identity == null)
        {
            var message = "Principal does not contain Identity Claims";
            return Result.Fail(new AuthenticationTokenIncorrectError(message));
        }

        var claimsIdentity = (ClaimsIdentity)principal.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
        {
            var message = "Identifier Claim is not found";
            return Result.Fail(new AuthenticationTokenIncorrectError(message));
        }

        return Result.Ok(claim.Value);
    }

    public static string GetIdentityUserId(this IPrincipal principal)
    {
        var identityUserIdResult = TryGetIdentityUserId(principal);
        if (identityUserIdResult.IsFailed)
        {
            var message = identityUserIdResult.Errors[0].Message;
            throw new AuthenticationTokenIncorrectException(message);
        }

        return identityUserIdResult.Value;
    }

    public static Result<Guid> TryGetApplicationUserId(this IPrincipal principal)
    {
        var identityUserIdResult = TryGetIdentityUserId(principal);
        if (identityUserIdResult.IsFailed)
        {
            return Result.Fail(identityUserIdResult.Errors);
        }

        var identityUserId = identityUserIdResult.Value;
        var applicationUserId = new Guid(identityUserId);

        return Result.Ok(applicationUserId);
    }

    public static Guid GetApplicationUserId(this IPrincipal principal)
    {
        var applicationUserIdResult = TryGetApplicationUserId(principal);
        if (applicationUserIdResult.IsFailed)
        {
            var message = applicationUserIdResult.Errors[0].Message;
            throw new AuthenticationTokenIncorrectException(message);
        }

        return applicationUserIdResult.Value;
    }
}