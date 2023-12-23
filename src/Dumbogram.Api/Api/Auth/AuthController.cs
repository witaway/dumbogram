using System.IdentityModel.Tokens.Jwt;
using Dumbogram.Api.Application.UseCases.Auth.Commands.SignIn;
using Dumbogram.Api.Application.UseCases.Auth.Commands.SignUp;
using Dumbogram.Api.Common.Controller;
using Dumbogram.Api.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Auth;

[ApiController]
[Route("api/[controller]", Name = "Auth")]
public class AuthController(IMediator mediator) : ApplicationController
{
    [HttpPost("sign-in", Name = nameof(SignIn))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<SignInResponse>))]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);

        var accessToken = result.Value.token;

        var accessTokenStringRepresentation = new JwtSecurityTokenHandler().WriteToken(accessToken);

        return Ok(new Responses.SignInResponse
        {
            Token = accessTokenStringRepresentation,
            Expiration = accessToken.ValidTo
        });
    }

    [HttpPost("sign-up", Name = nameof(SignUp))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseFailure))]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);

        return Ok();
    }
}