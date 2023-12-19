using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dumbogram.Api.Infrasctructure.Controller;

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
    protected virtual ObjectResult Failure([ActionResultObjectValue] List<IError> errors)
    {
        return new FailureObjectResult(errors);
    }

    [NonAction]
    protected virtual ObjectResult Failure([ActionResultObjectValue] IError error)
    {
        return new FailureObjectResult(error);
    }
}