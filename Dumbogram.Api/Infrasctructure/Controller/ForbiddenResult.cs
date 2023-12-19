using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Dumbogram.Api.Infrasctructure.Controller;

/// <summary>
///     An <see cref="StatusCodeResult" /> that when executed will produce an empty
///     <see cref="StatusCodes.Status403Forbidden" /> response.
/// </summary>
[DefaultStatusCode(DefaultStatusCode)]
public class ForbiddenResult : StatusCodeResult
{
    private const int DefaultStatusCode = StatusCodes.Status403Forbidden;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ForbiddenResult" /> class.
    /// </summary>
    public ForbiddenResult()
        : base(DefaultStatusCode)
    {
    }
}