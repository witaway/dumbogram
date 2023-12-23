using Dumbogram.Api.Application.Errors.Auth;
using Dumbogram.Api.Common.Extensions;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Identity;
using Dumbogram.Api.Persistence.Context.Identity.Entities;
using Dumbogram.Api.Persistence.Context.Identity.Enumerations;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Api.Application.UseCases.Auth.Commands.SignUp;

public class SignUpHandler(
    IdentityUserService identityUserService,
    UserManager<ApplicationIdentityUser> userManager,
    IdentityRolesService identityRolesService,
    ApplicationIdentityDbContext identityDbContext,
    ApplicationDbContext dbContext,
    UserService userService
) : IRequestHandler<SignUpRequest, Result>
{
    public async Task<Result> Handle(SignUpRequest request, CancellationToken cancellationToken)
    {
        // Try to create identity user
        // If failed - rollback transaction
        await using var identityDbTransaction = await identityDbContext
            .Database
            .BeginTransactionAsync(cancellationToken);

        var identityUserCreationResult = await CreateIdentityUser(request);
        if (identityUserCreationResult.IsFailed)
        {
            await identityDbTransaction.RollbackAsync(cancellationToken);
            return Result.Fail(identityUserCreationResult.Errors);
        }

        var identityUser = identityUserCreationResult.Value;

        // Try to create application user
        // If failed - rollback both transactions
        // That's because ApplicationIdentityUser and UserProfiles in different databases, but still related
        await using var applicationDbTransaction = await dbContext
            .Database
            .BeginTransactionAsync(cancellationToken);

        var applicationUserCreationResult = await CreateApplicationUser(request, identityUser);
        if (applicationUserCreationResult.IsFailed)
        {
            await identityDbTransaction.RollbackAsync(cancellationToken);
            await applicationDbTransaction.RollbackAsync(cancellationToken);
            return Result.Fail(applicationUserCreationResult.Errors);
        }

        // If we're here - all's good and user created!
        await identityDbTransaction.CommitAsync(cancellationToken);
        await applicationDbTransaction.CommitAsync(cancellationToken);

        return Result.Ok();
    }

    private async Task<Result<UserProfile>> CreateApplicationUser(
        SignUpRequest request,
        ApplicationIdentityUser identityUser
    )
    {
        UserProfile userProfile = new()
        {
            UserId = new Guid(identityUser.Id),
            Username = identityUser.UserName!
        };

        var usernameAlreadyTaken = await userService.IsUserProfileWithUsernameExist(userProfile.Username);
        if (usernameAlreadyTaken) return Result.Fail(new UsernameAlreadyTakenError());

        await userService.CreateUserProfile(userProfile);

        return userProfile;
    }

    private async Task<Result<ApplicationIdentityUser>> CreateIdentityUser(SignUpRequest request)
    {
        // Creating identity user
        ApplicationIdentityUser identityUser = new()
        {
            Email = request.Email,
            UserName = request.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        // Check existence of user
        var userExistenceResult = await CheckIdentityUserExistence(identityUser);
        if (userExistenceResult.IsFailed) return Result.Fail(userExistenceResult.Errors);

        // Trying to create IdentityUser
        var userCreationResult = await userManager.CreateAsync(identityUser, request.Password);
        if (!userCreationResult.Succeeded) return Result.Fail(userCreationResult.GetWrappedErrors());

        // Granting basic role to IdentityUser
        await identityRolesService.EnsureUserIsInRole(identityUser, UserRoles.User);

        return identityUser;
    }

    private async Task<Result> CheckIdentityUserExistence(ApplicationIdentityUser identityUser)
    {
        var existenceErrors = new List<IError>();

        var email = identityUser.Email!;
        var emailAlreadyTaken = await identityUserService.IsUserWithEmailExist(email);
        if (emailAlreadyTaken) existenceErrors.Add(new EmailAlreadyTakenError());

        var username = identityUser.UserName!;
        var usernameAlreadyTaken = await identityUserService.IsUserWithUsernameExist(username);
        if (usernameAlreadyTaken) existenceErrors.Add(new UsernameAlreadyTakenError());

        return existenceErrors.Any()
            ? Result.Fail(existenceErrors)
            : Result.Ok();
    }
}