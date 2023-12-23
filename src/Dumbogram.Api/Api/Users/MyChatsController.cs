using Dumbogram.Api.Api.Chats.Responses;
using Dumbogram.Api.Application.UseCases.Chats.Queries.GetMyJoinedChats;
using Dumbogram.Api.Application.UseCases.Chats.Queries.GetMyOwnedChats;
using Dumbogram.Api.Common.Controller;
using Dumbogram.Api.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Users;

[Authorize]
[Route("/api/users/me/chats", Name = "My chats")]
[ApiController]
public class MyChatsController(Mediator mediator) : ApplicationController
{
    [ProducesResponseType(
        StatusCodes.Status200OK,
        Type = typeof(ResponseSuccess<MultipleChatsShortInfoResponse>)
    )]
    [HttpGet("owned", Name = nameof(ReadOwnedChats))]
    public async Task<IActionResult> ReadOwnedChats()
    {
        var request = new GetMyOwnedChatsRequest();
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var chats = result.Value;

        var chatsDto = new MultipleChatsShortInfoResponse(chats);

        return Ok(chatsDto);
    }

    [ProducesResponseType(
        StatusCodes.Status200OK,
        Type = typeof(ResponseSuccess<MultipleChatsShortInfoResponse>)
    )]
    [HttpGet("joined", Name = nameof(ReadJoinedChats))]
    public async Task<IActionResult> ReadJoinedChats()
    {
        var request = new GetMyJoinedChatsRequest();
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var chats = result.Value;

        var chatsDto = new MultipleChatsShortInfoResponse(chats);

        return Ok(chatsDto);
    }
}