using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dumbogram.Common.Extensions;
using Dumbogram.Core.Auth.Controllers;
using Dumbogram.Core.Auth.Errors;
using Dumbogram.Core.Users.Models;
using Dumbogram.Core.Users.Services;
using Dumbogram.Database.Identity;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Core.Auth.Services;

public class AuthService
{
    private readonly IdentityRolesService _identityRolesService;
    private readonly IdentityUserService _identityUserService;
    private readonly ILogger<AuthController> _logger;
    private readonly TokenService _tokenService;
    private readonly UserManager<ApplicationIdentityUser> _userManager;
    private readonly UserService _userService;

    public AuthService(
        UserManager<ApplicationIdentityUser> userManager,
        ILogger<AuthController> logger,
        TokenService tokenService,
        IdentityUserService identityUserService,
        IdentityRolesService identityRolesService,
        UserService userService
    )
    {
        _userManager = userManager;
        _logger = logger;
        _tokenService = tokenService;
        _identityUserService = identityUserService;
        _identityRolesService = identityRolesService;
        _userService = userService;
    }

    private Task<bool> IsPasswordMatches(ApplicationIdentityUser user, string password)
    {
        return _userManager.CheckPasswordAsync(user, password);
    }

    private async Task<Result<IEnumerable<Claim>>> GetUserClaims(ApplicationIdentityUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
            .Append(new Claim(ClaimTypes.NameIdentifier, user.Id))
            .Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()))
            .Concat(
                userRoles.Select(role => new Claim(ClaimTypes.Role, role))
            );

        return Result.Ok(claims);
    }

    public async Task<Result<JwtSecurityToken>> SignIn(ApplicationIdentityUser user, string password)
    {
        // Checking if password matches user
        if (!await IsPasswordMatches(user, password))
        {
            return Result.Fail<JwtSecurityToken>(new PasswordNotValidError("Password is not valid"));
        }

        // Get user's claims
        var claimsResult = await GetUserClaims(user);

        if (claimsResult.IsFailed)
        {
            return Result.Fail<JwtSecurityToken>(claimsResult.Errors);
        }

        // Get token from claims
        var claims = claimsResult.Value;
        var tokenResult = _tokenService.CreateJwtSecurityToken(claims);

        return tokenResult.ToResult<JwtSecurityToken>();
    }

    public async Task<Result> SignUp(ApplicationIdentityUser user, string password)
    {
        // Ensure we have correct input
        if (user.Email == null || user.UserName == null)
        {
            throw new ArgumentNullException(nameof(user), "Email or UserName is null");
        }

        // User existence check
        var existenceErrors = new List<IError>();

        var emailAlreadyTaken = await _identityUserService.IsUserWithEmailExist(user.Email);
        if (emailAlreadyTaken)
        {
            existenceErrors.Add(new EmailAlreadyTakenError($"Email {user.Email} is already taken"));
        }

        var usernameAlreadyTaken = await _identityUserService.IsUserWithUsernameExist(user.UserName);
        if (usernameAlreadyTaken)
        {
            existenceErrors.Add(new UsernameAlreadyTakenError($"Username {user.UserName} is already taken"));
        }

        if (existenceErrors.Any())
        {
            return Result.Fail(existenceErrors);
        }

        // Trying to create IdentityUser
        var userCreationResult = await _userManager.CreateAsync(user, password);
        if (!userCreationResult.Succeeded)
        {
            return Result.Fail(userCreationResult.GetWrappedErrors());
        }

        // Granting basic role to IdentityUser
        await _identityRolesService.EnsureUserIsInRole(user, UserRoles.User);

        // Creating related UserProfile with no additional data
        UserProfile userProfile = new()
        {
            UserId = new Guid(user.Id),
            Username = user.UserName
        };

        await _userService.CreateUserProfile(userProfile);

        return Result.Ok();
    }
}