using Dumbogram.Api.Persistence.Context.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Api.Application.Users.Services;

public class IdentityRolesService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationIdentityUser> _userManager;

    public IdentityRolesService(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationIdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task EnsureUserIsInRole(ApplicationIdentityUser user, string role)
    {
        // We dont care if user already in role or not
        // We care only fact that user MUST be in role after the call
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task EnsureUserIsNotInRole(ApplicationIdentityUser user, string role)
    {
        // A similar like with GrantRoleToUser
        await _userManager.RemoveFromRoleAsync(user, role);
    }

    public Task<IList<string>> ReadUserRoles(ApplicationIdentityUser user)
    {
        return _userManager.GetRolesAsync(user);
    }

    public IQueryable<IdentityRole> ReadRoles()
    {
        return _roleManager.Roles;
    }
}