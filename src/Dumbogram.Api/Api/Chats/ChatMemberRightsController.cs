using Dumbogram.Api.Api.Chats.Responses;
using Dumbogram.Api.Application.UseCases.ChatMember.Commands.ApplyChatMemberRights;
using Dumbogram.Api.Application.UseCases.ChatMember.Queries.GetChatMemberRights;
using Dumbogram.Api.Common.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Chats;

[Authorize]
[Route("/api/chats/{chatId:guid}/members/{memberId:guid}", Name = "Rights")]
[ApiController]
public class ChatMemberRightsController(Mediator mediator) : ApplicationController
{
    [HttpGet(Name = nameof(GetRightsOfMember))]
    public async Task<IActionResult> GetRightsOfMember(Guid chatId, Guid memberId)
    {
        var request = new GetChatMemberRightsRequest(chatId, memberId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var rights = result.Value;

        var rightsDto = new MultipleRightsResponse(rights);
        return Ok(rightsDto);
    }


    [HttpPut(Name = nameof(ApplyRightsToMember))]
    public async Task<IActionResult> ApplyRightsToMember(
        Guid chatId,
        Guid memberId,
        [FromBody] List<string> rightsNames
    )
    {
        var request = new ApplyChatMemberRightsRequest(chatId, memberId, rightsNames);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);

        return Ok();
    }
}