using System.IdentityModel.Tokens.Jwt;

namespace Dumbogram.Api.Application.UseCases.Auth.Commands.SignIn;

public record SignInResponse(JwtSecurityToken token);