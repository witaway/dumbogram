using dumbogram.Models;
using Microsoft.AspNetCore.Mvc;

namespace dumbogram.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> logger;
    private readonly ApplicationDbContext dbContext;
    
    public UsersController(ILogger<UsersController> logger, ApplicationDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }
}