using Dumbogram.Application.Chats.Controllers.Dto;
using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Infrasctructure.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}/members", Name = "Members")]
[ApiController]
public class ChatMembersController : ApplicationController
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly UserResolverService _userResolverService;

    public ChatMembersController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        ILogger<ChatsController> logger,
        UserResolverService userResolverService
    )
    {
        _chatService = chatService;
        _chatMembershipService = chatMembershipService;
        _userResolverService = userResolverService;
        _logger = logger;
    }

    [HttpGet(Name = nameof(GetMembers))]
    public async Task<IActionResult> GetMembers(Guid chatId)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var members = await _chatMembershipService.ReadAllChatJoinedUsers(chat);
        var membersDto = new ReadMultipleMembersShortInfoResponse(members);

        return Ok(membersDto);
    }

    [HttpGet("banned", Name = nameof(GetBanned))]
    public async Task<IActionResult> GetBanned(Guid chatId)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var members = await _chatMembershipService.ReadAllChatJoinedUsers(chat);
        var membersDto = new ReadMultipleMembersShortInfoResponse(members);

        return Ok(membersDto);
    }
}