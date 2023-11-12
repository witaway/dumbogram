using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Core.Users.Services;

public class RolesService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RolesService(
        RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public void GrantRoleToUser(IdentityUser user, string role)
    {
        // We dont care if user already in role or not
        // We care only fact that user MUST be in role after the call
        _userManager.AddToRoleAsync(user, role);
    }

    public void RevokeRoleFromUser(IdentityUser user, string role)
    {
        // A similar like with GrantRoleToUser
        _userManager.RemoveFromRoleAsync(user, role);
    }

    public Task<IList<string>> ReadUserRoles(IdentityUser user)
    {
        return _userManager.GetRolesAsync(user);
    }

    public IQueryable<IdentityRole> ReadRoles()
    {
        return _roleManager.Roles;
    }
}