using Dumbogram.Api.Api.Messages.Responses;
using Dumbogram.Api.Application.UseCases.ChatMessage.Commands.DeleteChatMessage;
using Dumbogram.Api.Application.UseCases.ChatMessage.Commands.SendChatMessage;
using Dumbogram.Api.Application.UseCases.ChatMessage.Queries.GetChatMessage;
using Dumbogram.Api.Common.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Messages;

[Authorize]
[Route("/api/chats/{chatId:guid}/messages", Name = "Messages")]
[ApiController]
public class MessageController(IMediator mediator) : ApplicationController
{
    [HttpGet("{messageId:int}", Name = nameof(ReadSingleMessage))]
    public async Task<IActionResult> ReadSingleMessage(Guid chatId, int messageId)
    {
        var request = new GetChatMessageRequest(chatId, messageId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var message = result.Value;

        var messageDto = new SingleMessageResponse(message);
        return Ok(messageDto);
    }

    [HttpPost(Name = nameof(SendSingleMessage))]
    public async Task<IActionResult> SendSingleMessage(
        Guid chatId,
        [FromBody] SendChatMessageRequestContent messageContent
    )
    {
        var request = new SendChatMessageRequest(chatId, messageContent);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var message = result.Value;

        var messageDto = new SingleMessageResponse(message);

        return CreatedAtAction(
            nameof(ReadSingleMessage),
            new { chatId = message.ChatId, messageId = message.Id },
            messageDto
        );
    }

    [HttpDelete("{messageId:int}", Name = nameof(DeleteSingleMessage))]
    public async Task<IActionResult> DeleteSingleMessage(Guid chatId, int messageId)
    {
        var request = new DeleteChatMessageRequest(chatId, messageId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);

        return NoContent();
    }
}