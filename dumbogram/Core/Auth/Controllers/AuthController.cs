using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using Dumbogram.Common.Dto;
using Dumbogram.Common.Extensions;
using Dumbogram.Common.Filters;
using Dumbogram.Core.Auth.Dto;
using Dumbogram.Core.Auth.Errors;
using Dumbogram.Core.Auth.Services;
using Dumbogram.Core.Users.Services;
using Dumbogram.Database.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IdentityRolesService _identityRolesService;
    private readonly IdentityUserService _identityUserService;
    private readonly ILogger<AuthController> _logger;
    private readonly UserService _userService;

    public AuthController(
        IdentityUserService identityUserService,
        AuthService authService,
        IdentityRolesService identityRolesService,
        UserService userService,
        ILogger<AuthController> logger)
    {
        _identityUserService = identityUserService;
        _authService = authService;
        _identityRolesService = identityRolesService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    [Route("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestDto dto)
    {
        var user = dto switch
        {
            { Email: not null } => await _identityUserService.ReadUserByEmail(dto.Email),
            { Username: not null } => await _identityUserService.ReadUserByUsername(dto.Username),
            _ => throw new SwitchExpressionException()
        };

        if (user == null)
        {
            return Unauthorized(ResponseDto.Failure("User does not exist"));
        }

        var signInResult = await _authService.SignIn(user, dto.Password);

        if (signInResult.IsFailed)
        {
            return Unauthorized(signInResult.ToResult().ToFailureDto("Cannot sign in"));
        }

        var token = signInResult.Value;
        var tokenStringRepresentation = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            Status = "Success",
            Message = "Signed in successfully",
            Data = new
            {
                Token = tokenStringRepresentation,
                Expiration = token.ValidTo
            }
        });
    }

    [DevOnly]
    [HttpPost]
    [Route("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto dto)
    {
        ApplicationIdentityUser user = new()
        {
            Email = dto.Email,
            UserName = dto.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var signUpResult = await _authService.SignUp(user, dto.Password);

        return signUpResult switch
        {
            { IsSuccess: true } => Ok(new
            {
                Status = "Success",
                Message = "User created successfully!"
            }),
            { IsSuccess: false, Errors: [CredentialsConflictError, _] } => Conflict(
                signUpResult.ToFailureDto("User with such credentials already exist")),
            { IsSuccess: false, Errors: [WrappedIdentityError, _] } => BadRequest(
                signUpResult.ToFailureDto("Error during signing up")),
            _ => throw new SwitchExpressionException()
        };
    }

    [HttpPost]
    [Route("sign-up-admin")]
    public async Task<IActionResult> SignUpAdmin([FromBody] SignUpRequestDto dto)
    {
        // Reuse Signing Up code
        var result = await SignUp(dto);

        // If user is not created (result is not Ok), return this error
        // Otherwise continue, add role and return the same result.
        var okResult = result as OkObjectResult;
        if (okResult == null || okResult.StatusCode != 200)
        {
            return result;
        }

        // We know that user exist because already created it
        var user = (await _identityUserService.ReadUserByEmail(dto.Email))!;

        await _identityRolesService.EnsureUserIsInRole(user, UserRoles.Admin);

        return Ok(new
        {
            Status = "Success",
            Message = "Administrative User created successfully!"
        });
    }
}