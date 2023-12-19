using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using Dumbogram.Api.Application.Auth.Controllers.Dto;
using Dumbogram.Api.Application.Auth.Services;
using Dumbogram.Api.Application.Users.Services;
using Dumbogram.Api.Database.Identity;
using Dumbogram.Api.Infrasctructure.Controller;
using Dumbogram.Api.Infrasctructure.Dto;
using Dumbogram.Api.Infrasctructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Application.Auth.Controllers;

[ApiController]
[Route("api/[controller]", Name = "Auth")]
public class AuthController : ApplicationController
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

    [HttpPost("sign-in", Name = nameof(SignIn))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<SignInResponse>))]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest dto)
    {
        var userResult = dto switch
        {
            { Email: not null } => await _identityUserService.RequestUserByEmail(dto.Email),
            { Username: not null } => await _identityUserService.RequestUserByUsername(dto.Username),
            _ => throw new SwitchExpressionException()
        };
        if (userResult.IsFailed)
        {
            return Failure(userResult.Errors);
        }

        var signInResult = await _authService.SignIn(userResult.Value, dto.Password);
        if (signInResult.IsFailed)
        {
            return Failure(signInResult.Errors);
        }

        var token = signInResult.Value;
        var tokenStringRepresentation = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new SignInResponse
        {
            Token = tokenStringRepresentation,
            Expiration = token.ValidTo
        });
    }

    [HttpPost("sign-up", Name = nameof(SignUp))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseFailure))]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest dto)
    {
        ApplicationIdentityUser user = new()
        {
            Email = dto.Email,
            UserName = dto.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var signUpResult = await _authService.SignUp(user, dto.Password);
        if (signUpResult.IsFailed)
        {
            return Failure(signUpResult.Errors);
        }

        return Ok();
    }

    [DevOnly]
    [HttpPost("sign-up-admin", Name = nameof(SignUpAdmin))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseFailure))]
    public async Task<IActionResult> SignUpAdmin([FromBody] SignUpRequest dto)
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

        return Ok();
    }
}