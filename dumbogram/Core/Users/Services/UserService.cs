using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Core.Users.Services;

public class UserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityUser?> ReadUserByUsername(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public async Task<IdentityUser?> ReadUserByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityUser?> ReadUserById(string userId)
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