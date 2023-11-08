using Dumbogram.Common.Filters;
using Dumbogram.Core.Auth.Dto;
using Dumbogram.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Test;

[DevOnly]
[ApiController]
[Route("/api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<TestController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public TestController(ILogger<TestController> logger, ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
    }


    [HttpPost(Name = "Echo")]
    [Route("echo")]
    public async Task<IActionResult> SignIn([FromBody] object model)
    {
        return Ok(model);
    }

    [HttpPost(Name = "EchoValidate")]
    [Route("echo-validate")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestDto model)
    {
        return Ok(model);
    }

    [Authorize]
    [HttpPost(Name = "GetCurrentUser")]
    [Route("user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        return Ok(user);
    }

    private class GetCurrentUserResponseDto
    {
    }
}