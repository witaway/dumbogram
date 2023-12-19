using System.Security.Claims;
using Dumbogram.Api.Application.Users.Services.Errors;
using Dumbogram.Api.Application.Users.Services.Exceptions;
using Dumbogram.Api.Database.Identity;
using Dumbogram.Api.Infrasctructure.Extensions;
using Dumbogram.Api.Models.Users;
using FluentResults;

namespace Dumbogram.Api.Application.Users.Services;

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

    public async Task<UserProfile> GetApplicationUser()
    {
        var applicationUserResult = await TryGetApplicationUser();

        if (applicationUserResult.IsFailed)
        {
            throw new UnauthorizedException();
        }

        return applicationUserResult.Value;
    }

    public async Task<ApplicationIdentityUser> GetIdentityUser()
    {
        var applicationUserResult = await TryGetIdentityUser();

        if (applicationUserResult.IsFailed)
        {
            throw new UnauthorizedException();
        }

        return applicationUserResult.Value;
    }
}