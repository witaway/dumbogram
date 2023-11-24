using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dumbogram.Common.Controller;

public class ApplicationController : ControllerBase
{
    [NonAction]
    public virtual ForbiddenResult Forbidden()
    {
        return new ForbiddenResult();
    }

    [NonAction]
    public virtual ForbiddenObjectResult Forbidden([ActionResultObjectValue] object? value)
    {
        return new ForbiddenObjectResult(value);
    }
}