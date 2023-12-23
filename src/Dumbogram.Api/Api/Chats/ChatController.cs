using Dumbogram.Api.Api.Chats.Responses;
using Dumbogram.Api.Application.UseCases.Chat.Commands.CreateChat;
using Dumbogram.Api.Application.UseCases.Chat.Commands.JoinChat;
using Dumbogram.Api.Application.UseCases.Chat.Commands.LeaveChat;
using Dumbogram.Api.Application.UseCases.Chat.Queries.ReadChat;
using Dumbogram.Api.Common.Controller;
using Dumbogram.Api.Common.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Chats;

[Authorize]
[Route("/api/chats", Name = "Chats")]
[ApiController]
public class ChatController(IMediator mediator) : ApplicationController
{
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseSuccess))]
    [HttpPost(Name = nameof(CreateChat))]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
    {
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var chat = result.Value;

        var chatDto = new SingleChatShortInfoResponse(chat);
        return CreatedAtAction(nameof(ReadChat), new { chatId = chat.Id }, chatDto);
    }

    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<SingleChatShortInfoResponse>))]
    [HttpGet(Name = nameof(ReadChat))]
    public async Task<IActionResult> ReadChat([FromRoute] Guid chatId)
    {
        var request = new ReadChatRequest(chatId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var chat = result.Value;

        var chatDto = new SingleChatShortInfoResponse(chat);
        return Ok(chatDto);
    }

    [HttpGet("join", Name = nameof(JoinChat))]
    public async Task<IActionResult> JoinChat([FromRoute] Guid chatId)
    {
        var request = new JoinChatRequest(chatId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);

        return Ok();
    }

    [HttpGet("leave", Name = nameof(LeaveChat))]
    public async Task<IActionResult> LeaveChat([FromRoute] Guid chatId)
    {
        var request = new LeaveChatRequest(chatId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);

        return Ok();
    }
}