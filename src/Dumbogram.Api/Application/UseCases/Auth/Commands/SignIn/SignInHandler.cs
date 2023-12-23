using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Dumbogram.Api.Application.Errors.Auth;
using Dumbogram.Api.Domain.Services.Tokens;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Persistence.Context.Identity.Entities;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Api.Application.UseCases.Auth.Commands.SignIn;

public class SignInHandler(
    IdentityUserService identityUserService,
    TokenService tokenService,
    UserManager<ApplicationIdentityUser> userManager
) : IRequestHandler<SignInRequest, Result<SignInResponse>>
{
    public async Task<Result<SignInResponse>> Handle(SignInRequest request, CancellationToken cancellationToken)
    {
        // Retrieve identity user by email or username conditionally
        var retrieveIdentityUserResult = request switch
        {
            { Email: not null } => await identityUserService.RequestUserByEmail(request.Email),
            { Username: not null } => await identityUserService.RequestUserByUsername(request.Username),
            _ => throw new SwitchExpressionException()
        };
        if (retrieveIdentityUserResult.IsFailed) return Result.Fail(retrieveIdentityUserResult.Errors);

        var identityUser = retrieveIdentityUserResult.Value;

        // Checking if password matches user
        var password = request.Password;
        if (!await IsPasswordMatches(identityUser, password)) return Result.Fail(new PasswordNotValidError());

        // Get user's claims
        var claimsResult = await GetUserClaims(identityUser);
        if (claimsResult.IsFailed) return Result.Fail(claimsResult.Errors);
        var claims = claimsResult.Value;

        // Get token from claims
        var tokenResult = tokenService.CreateJwtSecurityToken(claims);
        if (tokenResult.IsFailed) return Result.Fail(tokenResult.Errors);

        // Return token and we're cool!
        var token = tokenResult.Value;
        var signInResponse = new SignInResponse(token);

        return signInResponse;
    }

    private Task<bool> IsPasswordMatches(ApplicationIdentityUser user, string password)
    {
        return userManager.CheckPasswordAsync(user, password);
    }

    private async Task<Result<IEnumerable<Claim>>> GetUserClaims(ApplicationIdentityUser user)
    {
        var userRoles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
            .Append(new Claim(ClaimTypes.NameIdentifier, user.Id))
            .Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()))
            .Concat(
                userRoles.Select(role => new Claim(ClaimTypes.Role, role))
            );

        return Result.Ok(claims);
    }
}