using Dumbogram.Api.Application.Auth.Controllers.Dto;
using Dumbogram.Api.Application.Users.Controllers.Dto;
using Dumbogram.Api.Infrasctructure.Dto;
using Dumbogram.Api.Infrasctructure.Extensions;
using Dumbogram.Api.Infrasctructure.Filters;
using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Application.Test;

public class A
{
    public int Q { get; set; } = 1;
    public int W { get; set; } = 2;
}

public class B : A
{
    public int E { get; set; } = 3;
}

[DevOnly]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<TestController> _logger;
    private readonly UserManager<ApplicationIdentityUser> _userManager;

    public TestController(ILogger<TestController> logger, ApplicationDbContext dbContext,
        UserManager<ApplicationIdentityUser> userManager)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
    }


    [HttpPost("echo", Name = "Echo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<object>))]
    public IActionResult Echo([FromBody] object model)
    {
        return Ok(model);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<SignInRequest>))]
    [HttpPost("echo-validate")]
    public IActionResult EchoValidate([FromBody] SignInRequest model)
    {
        return Ok(model);
    }

    [HttpGet("seppuku", Name = "UnhandledException")]
    public IActionResult UnhandledException()
    {
        throw new Exception("Application has committed seppuku just now!");
    }

    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<GetIdentityUserByUserIdResponse>)
    )]
    [Authorize]
    [HttpPost("me", Name = "GetCurrentUser")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userManager.FindByIdAsync(User.GetIdentityUserId());
        var userDto = GetIdentityUserByUserIdResponse.MapFromModel(user!);
        return Ok(userDto);
    }

    [HttpGet("supclass")]
    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<GetIdentityUserByUserIdResponse>)
    )]
    public async Task<IActionResult> Supclass()
    {
        A test = new B();
        return Ok(test);
    }
}