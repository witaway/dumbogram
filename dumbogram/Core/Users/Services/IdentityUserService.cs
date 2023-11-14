using Dumbogram.Database.Identity;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Core.Users.Services;

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

    public async Task<ApplicationIdentityUser?> ReadUserByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationIdentityUser?> ReadUserById(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
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