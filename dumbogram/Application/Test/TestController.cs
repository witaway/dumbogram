﻿using Dumbogram.Application.Auth.Dto;
using Dumbogram.Application.Users.Dto;
using Dumbogram.Common.Dto;
using Dumbogram.Common.Extensions;
using Dumbogram.Common.Filters;
using Dumbogram.Database;
using Dumbogram.Database.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Test;

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


    [HttpPost(Name = "Echo")]
    [Route("echo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<object>))]
    public IActionResult Echo([FromBody] object model)
    {
        return Ok(Common.Dto.Response.Success(model));
    }

    [HttpPost(Name = "EchoValidate")]
    [Route("echo-validate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<SignInRequestDto>))]
    public IActionResult EchoValidate([FromBody] SignInRequestDto model)
    {
        return Ok(Common.Dto.Response.Success(model));
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
        return Ok(Common.Dto.Response.Success(userDto));
    }
}