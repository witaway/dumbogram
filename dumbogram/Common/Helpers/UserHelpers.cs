using System.Security.Claims;
using System.Security.Principal;

namespace Dumbogram.Common.Helpers;

public class CannotExtractClaimException : Exception
{
    public CannotExtractClaimException()
    {
    }

    public CannotExtractClaimException(string message)
        : base(message)
    {
    }

    public CannotExtractClaimException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public static class UserHelpers
{
    public static string GetUserIdentityId(this IPrincipal principal)
    {
        if (principal.Identity == null)
        {
            throw new CannotExtractClaimException("[Authorization] Principal does not contain Identity Claims");
        }

        var claimsIdentity = (ClaimsIdentity)principal.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
        {
            throw new CannotExtractClaimException("[Authorization] Identifier Claim is not found");
        }

        return claim.Value;
    }

    public static Guid GetUserApplicationId(this IPrincipal principal)
    {
        return new Guid(GetUserIdentityId(principal));
    }
}