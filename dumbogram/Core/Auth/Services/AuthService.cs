using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Dumbogram.Core.Auth.Services;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenService _tokenService;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<AuthController> logger,
        TokenService tokenService
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task<bool> CheckPasswordCorrectness(IdentityUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    private async Task<IEnumerable<Claim>> GetUserClaims(IdentityUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
            .Append(new Claim(ClaimTypes.Name, user.UserName))
            .Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()))
            .Concat(
                userRoles.Select(role => new Claim(ClaimTypes.Role, role))
            );

        return claims;
    }

    public async Task<JwtSecurityToken> SignIn(IdentityUser user)
    {
        var claims = await GetUserClaims(user);
        var token = _tokenService.CreateJwtSecurityToken(claims);
        return token;
    }

    public async Task<IdentityResult> SignUp(IdentityUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result;
    }
}