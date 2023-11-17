using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentResults;
using Microsoft.IdentityModel.Tokens;

namespace Dumbogram.Core.Auth.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Result<JwtSecurityToken> CreateJwtSecurityToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var issuer = _configuration["JWT:ValidIssuer"];
        var audience = _configuration["JWT:ValidAudience"];
        var expires = DateTime.Now.AddHours(3);
        var signingCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            expires: expires,
            claims: claims,
            signingCredentials: signingCredentials
        );

        return Result.Ok(token);
    }
}