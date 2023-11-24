using Dumbogram.Common.Controller;
using Dumbogram.Common.Extensions;
using Dumbogram.Core.Chats.Dto;
using Dumbogram.Core.Chats.Services;
using Dumbogram.Core.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}/members")]
[ApiController]
public class ChatMembersController : ApplicationController
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly UserService _userService;

    public ChatMembersController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        UserService userService,
        ILogger<ChatsController> logger
    )
    {
        _chatService = chatService;
        _userService = userService;
        _chatMembershipService = chatMembershipService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetMembers(Guid chatId)
    {
        var uid = User.GetApplicationUserId();
        var userProfile = await _userService.ReadUserProfileById(uid);

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);

        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var members = await _chatMembershipService.ReadAllChatJoinedUsers(chat);

        var membersDto = new ReadMultipleMembersShortInfoResponseDto(members);
        return Ok(membersDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetBanned(Guid chatId)
    {
        var uid = User.GetApplicationUserId();
        var userProfile = await _userService.ReadUserProfileById(uid);


        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);

        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var members = await _chatMembershipService.ReadAllChatJoinedUsers(chat);

        var membersDto = new ReadMultipleMembersShortInfoResponseDto(members);
        return Ok(membersDto);
    }
}