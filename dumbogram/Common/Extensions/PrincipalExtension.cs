using System.Security.Claims;
using System.Security.Principal;
using Dumbogram.Common.Exceptions;

namespace Dumbogram.Common.Extensions;

public class CannotExtractClaimException : BaseApplicationException
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

public static class PrincipalExtension
{
    public static string GetUserIdentityId(this IPrincipal principal)
    {
        if (principal.Identity == null)
        {
            throw new CannotExtractClaimException("Principal does not contain Identity Claims");
        }

        var claimsIdentity = (ClaimsIdentity)principal.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
        {
            throw new CannotExtractClaimException("Identifier Claim is not found");
        }

        return claim.Value;
    }

    public static Guid GetUserApplicationId(this IPrincipal principal)
    {
        return new Guid(GetUserIdentityId(principal));
    }
}