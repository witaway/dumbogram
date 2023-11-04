using Dumbogram.Dto;
using Dumbogram.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    [HttpPost(Name = "SignIn")]
    public SignInDto SignIn([FromBody] SignInDto signInDto)
    {
        return signInDto;
    }
}