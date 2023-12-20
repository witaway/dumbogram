using Dumbogram.Api.Application.Users.Services.Errors;
using Dumbogram.Api.Persistence.Context.Identity.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Api.Application.Users.Services;

public class IdentityUserService
{
    private readonly UserManager<ApplicationIdentityUser> _userManager;

    public IdentityUserService(UserManager<ApplicationIdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationIdentityUser?> ReadUserByUsername(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<Result<ApplicationIdentityUser>> RequestUserByUsername(string username)
    {
        var identityUser = await ReadUserByUsername(username);
        if (identityUser == null) return Result.Fail(new UserNotFoundError());

        return Result.Ok(identityUser);
    }

    public async Task<ApplicationIdentityUser?> ReadUserByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<Result<ApplicationIdentityUser>> RequestUserByEmail(string email)
    {
        var identityUser = await ReadUserByEmail(email);
        if (identityUser == null) return Result.Fail(new UserNotFoundError());

        return Result.Ok(identityUser);
    }

    public async Task<ApplicationIdentityUser?> ReadUserById(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<Result<ApplicationIdentityUser>> RequestUserById(string userId)
    {
        var identityUser = await ReadUserById(userId);
        if (identityUser == null) return Result.Fail(new UserNotFoundError());

        return Result.Ok(identityUser);
    }

    public async Task<bool> IsUserWithUsernameExist(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user != null;
    }

    public async Task<bool> IsUserWithEmailExist(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }

    public async Task<bool> IsUserWithIdExist(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null;
    }
}