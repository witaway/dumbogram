using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using Dumbogram.Application.Auth.Dto;
using Dumbogram.Application.Auth.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Database.Identity;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Dto;
using Dumbogram.Infrasctructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
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

    [HttpPost]
    [Route("sign-in")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<SignInResponseDto>))]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestDto dto)
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

        return Ok(new SignInResponseDto
        {
            Token = tokenStringRepresentation,
            Expiration = token.ValidTo
        });
    }

    [HttpPost]
    [Route("sign-up")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseFailure))]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto dto)
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
    [HttpPost]
    [Route("sign-up-admin")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseFailure))]
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

        return Ok();
    }
}