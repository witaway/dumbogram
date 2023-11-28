using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dumbogram.Infrasctructure.Controller;

/// <summary>
///     An <see cref="ObjectResult" /> that when executed performs content negotiation, formats the entity body, and
///     will produce a <see cref="StatusCodes.Status403Forbidden" /> response if negotiation and formatting succeed.
/// </summary>
[DefaultStatusCode(DefaultStatusCode)]
public class ForbiddenObjectResult : ObjectResult
{
    private const int DefaultStatusCode = StatusCodes.Status403Forbidden;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ForbiddenObjectResult" /> class.
    /// </summary>
    /// <param name="value">The content to format into the entity body.</param>
    public ForbiddenObjectResult(object? value)
        : base(value)
    {
        StatusCode = DefaultStatusCode;
    }
}