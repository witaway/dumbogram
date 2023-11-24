using Dumbogram.Common.Controller;
using Dumbogram.Common.Dto;
using Dumbogram.Common.Extensions;
using Dumbogram.Core.Chats.Dto;
using Dumbogram.Core.Chats.Services;
using Dumbogram.Core.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}")]
[ApiController]
public class ChatController : ApplicationController
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatPermissionsService _chatPermissionsService;

    private readonly ChatService _chatService;

    private readonly ILogger<ChatsController> _logger;
    private readonly UserService _userService;

    public ChatController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        ChatPermissionsService chatPermissionsService,
        UserService userService,
        ILogger<ChatsController> logger
    )
    {
        _chatService = chatService;
        _chatPermissionsService = chatPermissionsService;
        _userService = userService;
        _chatMembershipService = chatMembershipService;
        _logger = logger;
    }

    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadSingleChatShortInfoResponseDto>))]
    [HttpGet]
    public async Task<IActionResult> ReadChat([FromRoute] Guid chatId)
    {
        var userProfile = await _userService.ReadUserProfileById(User.GetApplicationUserId());

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);

        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var chatDto = new ReadSingleChatShortInfoResponseDto(chatResult.Value);
        return Ok(chatDto);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeChatInfo([FromRoute] Guid chatId)
    {
        // Todo: Implement changing chat info
        throw new NotImplementedException();
    }

    [HttpGet("join")]
    public async Task<IActionResult> JoinChat([FromRoute] Guid chatId)
    {
        var uid = User.GetApplicationUserId();
        var userProfile = await _userService.ReadUserProfileById(uid);

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);

        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var joinResult = await _chatMembershipService.JoinUserToChat(userProfile!, chat);

        if (joinResult.IsFailed)
        {
            return Failure(joinResult.Errors);
        }

        return Ok();
    }

    [HttpGet("leave")]
    public async Task<IActionResult> LeaveChat([FromRoute] Guid chatId)
    {
        var uid = User.GetApplicationUserId();
        var userProfile = await _userService.ReadUserProfileById(uid);

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);

        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;

        var leaveResult = await _chatMembershipService.JoinUserToChat(userProfile!, chat!);

        if (leaveResult.IsFailed)
        {
            return Failure(leaveResult.Errors);
        }

        return Ok();
    }
}