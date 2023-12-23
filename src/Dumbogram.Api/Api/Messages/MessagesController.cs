using Dumbogram.Api.Api.Messages.Responses;
using Dumbogram.Api.Application.UseCases.ChatMessages.Queries.GetChatMessages;
using Dumbogram.Api.Common.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Messages;

[Authorize]
[Route("/api/chats/{chatId:guid}/messages", Name = "Messages")]
[ApiController]
public class MessagesController(IMediator mediator) : ApplicationController
{
    [HttpGet(Name = nameof(ReadMultipleMessages))]
    public async Task<IActionResult> ReadMultipleMessages(Guid chatId)
    {
        var request = new GetChatMessagesRequest(chatId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var messages = result.Value;

        var messagesDto = new MultipleMessagesResponse(messages);
        return Ok(messagesDto);
    }
}