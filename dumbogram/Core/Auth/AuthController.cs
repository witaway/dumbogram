using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using Dumbogram.Common.Filters;
using Dumbogram.Core.Auth.Dto;
using Dumbogram.Core.Auth.Services;
using Dumbogram.Core.User.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Auth;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Moderator = "Moderator";
}

public class Response
{
    public string? Status { get; set; }
    public string? Message { get; set; }
}

// TODO: REFACTOR ALL THE SHIT!!!!!!!! Separate code to layers such as a repository and service and make it less SHITTY.
// THIS IS ONLY A BASIC DEMONSTRATION. MUST BE REFACTORED.
// THIS IS NOT AN EVEN CLOSE FINAL VERSION.

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly RolesService _rolesService;
    private readonly UserService _userService;

    public AuthController(
        UserService userService,
        AuthService authService,
        RolesService rolesService,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _authService = authService;
        _rolesService = rolesService;
        _logger = logger;
    }

    [HttpPost]
    [Route("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestDto model)
    {
        var user = model switch
        {
            { Email: not null } => await _userService.ReadUserByEmail(model.Email),
            { Username: not null } => await _userService.ReadUserByUsername(model.Username),
            _ => throw new SwitchExpressionException()
        };

        if (user == null)
        {
            return Unauthorized();
        }

        var isPasswordValid = await _authService.CheckPasswordCorrectness(user, model.Password);
        if (!isPasswordValid)
        {
            return Unauthorized();
        }

        var token = await _authService.SignIn(user);
        var tokenStringRepresentation = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenStringRepresentation,
            expiration = token.ValidTo
        });
    }

    [DevOnly]
    [HttpPost]
    [Route("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto model)
    {
        var emailAlreadyTaken = await _userService.IsUserWithEmailExist(model.Email);
        var usernameAlreadyTaken = await _userService.IsUserWithUsernameExist(model.Username);

        if (emailAlreadyTaken)
        {
            return Conflict(new
            {
                Status = "Error",
                Message = "Email already taken"
            });
        }

        if (usernameAlreadyTaken)
        {
            return Conflict(new
            {
                Status = "Error",
                Message = "Username already taken"
            });
        }

        IdentityUser user = new()
        {
            Email = model.Email,
            UserName = model.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _authService.SignUp(user, model.Password);

        if (!result.Succeeded)
        {
            var details = result.Errors.Select(error => new
            {
                error.Code, error.Description
            });

            return BadRequest(new
            {
                Status = "Error",
                Message = "User creation failed",
                Details = details
            });
        }

        _rolesService.GrantRoleToUser(user, UserRoles.User);

        return Ok(new
            {
                Status = "Success",
                Message = "User created successfully!"
            }
        );
    }

    [HttpPost]
    [Route("sign-up-admin")]
    public async Task<IActionResult> SignUpAdmin([FromBody] SignUpRequestDto model)
    {
        // Reuse Signing Up code
        var result = await SignUp(model);

        // If user is not created (result is not Ok), return this error
        // Otherwise continue, add role and return the same result.
        var okResult = result as OkObjectResult;
        if (okResult == null || okResult.StatusCode != 200)
        {
            return result;
        }

        // We know that user exist because already created it
        var user = (await _userService.ReadUserByEmail(model.Email))!;

        _rolesService.GrantRoleToUser(user, UserRoles.Admin);

        return Ok(new
        {
            Status = "Success",
            Message = "Administrative User created successfully!"
        });
    }
}