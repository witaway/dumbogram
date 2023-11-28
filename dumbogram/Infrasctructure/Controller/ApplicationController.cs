using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dumbogram.Infrasctructure.Controller;

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

    [NonAction]
    public virtual ObjectResult Failure([ActionResultObjectValue] List<IError> errors)
    {
        return new FailureObjectResult(errors);
    }

    public virtual ObjectResult Failure([ActionResultObjectValue] IError error)
    {
        return new FailureObjectResult(error);
    }
}