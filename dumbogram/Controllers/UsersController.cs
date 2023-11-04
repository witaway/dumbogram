using Dumbogram.Dto;
using Dumbogram.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, ApplicationDbContext dbContext)
    {
        this._logger = logger;
        this._dbContext = dbContext;
    }


    [HttpPost(Name = "SignIn")]
    public SignInDto SignIn([FromBody] SignInDto signInDto)
    {
        return signInDto;
    }
}