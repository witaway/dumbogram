using System.Security.Claims;
using Dumbogram.Application.Users.Errors;
using Dumbogram.Application.Users.Models;
using Dumbogram.Common.Extensions;
using Dumbogram.Database.Identity;
using FluentResults;

namespace Dumbogram.Application.Users.Services;

public class UserResolverService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IdentityUserService _identityUserService;
    private readonly UserService _userService;

    public UserResolverService(
        IHttpContextAccessor httpContextAccessor,
        UserService userService,
        IdentityUserService identityUserService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _identityUserService = identityUserService;
    }

    private ClaimsPrincipal? GetClaimsPrincipal()
    {
        return _httpContextAccessor?.HttpContext?.User;
    }

    public async Task<Result<UserProfile>> TryGetApplicationUser()
    {
        var claimsPrincipal = GetClaimsPrincipal();
        if (claimsPrincipal == null)
        {
            return Result.Fail(new UnauthorizedError());
        }

        var applicationUserIdResult = claimsPrincipal.TryGetApplicationUserId();
        if (applicationUserIdResult.IsFailed)
        {
            return Result.Fail(applicationUserIdResult.Errors);
        }

        var applicationUserId = applicationUserIdResult.Value;

        var applicationUserResult = await _userService.RequestUserProfileById(applicationUserId);
        if (applicationUserResult.IsFailed)
        {
            return Result.Fail(applicationUserResult.Errors);
        }

        var applicationUser = applicationUserResult.Value;
        return applicationUser;
    }

    public async Task<UserProfile?> GetApplicationUser()
    {
        var applicationUserResult = await TryGetApplicationUser();

        return applicationUserResult.IsFailed
            ? null
            : applicationUserResult.Value;
    }

    public async Task<Result<ApplicationIdentityUser>> TryGetIdentityUser()
    {
        var claimsPrincipal = GetClaimsPrincipal();
        if (claimsPrincipal == null)
        {
            return Result.Fail(new UnauthorizedError());
        }

        var identityUserIdResult = claimsPrincipal.TryGetIdentityUserId();
        if (identityUserIdResult.IsFailed)
        {
            return Result.Fail(identityUserIdResult.Errors);
        }

        var identityUserId = identityUserIdResult.Value;

        var identityUserResult = await _identityUserService.RequestUserById(identityUserId);
        if (identityUserResult.IsFailed)
        {
            return Result.Fail(identityUserResult.Errors);
        }

        var identityUser = identityUserResult.Value;
        return identityUser;
    }

    public async Task<ApplicationIdentityUser?> GetIdentityUser()
    {
        var applicationUserResult = await TryGetIdentityUser();

        return applicationUserResult.IsFailed
            ? null
            : applicationUserResult.Value;
    }
}