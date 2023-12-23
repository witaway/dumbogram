using Dumbogram.Api.Api.Chats.Responses;
using Dumbogram.Api.Application.UseCases.ChatMembers.Queries.GetChatMembers;
using Dumbogram.Api.Application.UseCases.ChatMembers.Queries.GetChatMembersBanned;
using Dumbogram.Api.Common.Controller;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Chats;

[Authorize]
[Route("/api/chats/{chatId:guid}/members", Name = "Members")]
[ApiController]
public class ChatMembersController(IMediator mediator) : ApplicationController
{
    [HttpGet(Name = nameof(GetChatMembers))]
    public async Task<IActionResult> GetChatMembers(Guid chatId)
    {
        var request = new GetChatMembersRequest(chatId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var members = result.Value;

        var membersDto = new MultipleMembersShortInfoResponse(members);
        return Ok(membersDto);
    }


    [HttpGet("banned", Name = nameof(GetChatMembersBanned))]
    public async Task<IActionResult> GetChatMembersBanned(Guid chatId)
    {
        var request = new GetChatMembersBannedRequest(chatId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var banned = result.Value;

        var bannedDto = new MultipleMembersShortInfoResponse(banned);
        return Ok(bannedDto);
    }
}