using Dumbogram.Api.Api.Chats.Responses;
using Dumbogram.Api.Application.UseCases.Chats.Queries.SearchChats;
using Dumbogram.Api.Common.Controller;
using Dumbogram.Api.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Chats;

[Authorize]
[Route("/api/chats", Name = "Chats")]
[ApiController]
public class ChatsController(IMediator mediator) : ApplicationController
{
    [ProducesResponseType(
        statusCode: StatusCodes.Status200OK,
        type: typeof(ResponseSuccess<MultipleChatsShortInfoResponse>)
    )]
    [HttpGet("search", Name = nameof(SearchChats))]
    public async Task<IActionResult> SearchChats()
    {
        var request = new SearchChatsRequest();
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var chats = result.Value;

        var chatsDto = new MultipleChatsShortInfoResponse(chats);
        return Ok(chatsDto);
    }
}