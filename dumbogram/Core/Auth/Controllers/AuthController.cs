using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using Dumbogram.Common.Filters;
using Dumbogram.Core.Auth.Dto;
using Dumbogram.Core.Auth.Services;
using Dumbogram.Core.Users.Models;
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
            return Unauthorized();
        }

        var isPasswordValid = await _authService.CheckPasswordCorrectness(user, dto.Password);
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
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto dto)
    {
        // User existence check
        var emailAlreadyTaken = await _identityUserService.IsUserWithEmailExist(dto.Email);
        var usernameAlreadyTaken = await _identityUserService.IsUserWithUsernameExist(dto.Username);

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

        // Creating IdentityUser
        ApplicationIdentityUser user = new()
        {
            Email = dto.Email,
            UserName = dto.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _authService.SignUp(user, dto.Password);

        // Checking is IdentityUser created
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

        // Granting roles to IdentityUser
        _identityRolesService.GrantRoleToUser(user, UserRoles.User);

        // Creating related UserProfile
        var userId = new Guid(user.Id);
        UserProfile userProfile = new()
        {
            Username = dto.Username,
            Description = dto.Profile?.Description
        };
        _userService.CreateUserProfile(userId, userProfile);


        return Ok(new
            {
                Status = "Success",
                Message = "User created successfully!"
            }
        );
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

        _identityRolesService.GrantRoleToUser(user, UserRoles.Admin);

        return Ok(new
        {
            Status = "Success",
            Message = "Administrative User created successfully!"
        });
    }
}