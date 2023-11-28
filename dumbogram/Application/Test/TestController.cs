using Dumbogram.Application.Auth.Dto;
using Dumbogram.Application.Users.Dto;
using Dumbogram.Database;
using Dumbogram.Database.Identity;
using Dumbogram.Infrasctructure.Dto;
using Dumbogram.Infrasctructure.Extensions;
using Dumbogram.Infrasctructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Test;

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


    [Route("echo")]
    [HttpPost(Name = "Echo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<object>))]
    public IActionResult Echo([FromBody] object model)
    {
        return Ok(Infrasctructure.Dto.Response.Success(model));
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<SignInRequestDto>))]
    [HttpPost("echo-validate")]
    public IActionResult EchoValidate([FromBody] SignInRequestDto model)
    {
        return Ok(Infrasctructure.Dto.Response.Success(model));
    }

    [HttpGet(Name = "UnhandledException")]
    [Route("seppuku")]
    public IActionResult UnhandledException()
    {
        throw new Exception("Application has committed seppuku just now!");
    }

    [Authorize]
    [HttpPost(Name = "GetCurrentUser")]
    [Route("me")]
    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<GetIdentityUserByUserIdResponseDto>)
    )]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userManager.FindByIdAsync(User.GetIdentityUserId());
        var userDto = GetIdentityUserByUserIdResponseDto.MapFromModel(user!);
        return Ok(Infrasctructure.Dto.Response.Success(userDto));
    }

    [HttpGet("supclass")]
    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<GetIdentityUserByUserIdResponseDto>)
    )]
    public async Task<IActionResult> Supclass()
    {
        A test = new B();
        return Ok(test);
    }
}