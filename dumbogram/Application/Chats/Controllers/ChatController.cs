using Dumbogram.Application.Chats.Controllers.Dto;
using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Dto;
using Dumbogram.Models.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}", Name = "Chat")]
[ApiController]
public class ChatController : ApplicationController
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatPermissionsService _chatPermissionsService;
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly SystemMessagesService _systemMessagesService;
    private readonly UserResolverService _userResolverService;

    public ChatController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        ILogger<ChatsController> logger,
        UserResolverService userResolverService,
        ChatPermissionsService chatPermissionsService,
        SystemMessagesService systemMessagesService
    )
    {
        _chatService = chatService;
        _chatMembershipService = chatMembershipService;
        _userResolverService = userResolverService;
        _chatMembershipService = chatMembershipService;
        _chatPermissionsService = chatPermissionsService;
        _systemMessagesService = systemMessagesService;
        _logger = logger;
    }

    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadSingleChatShortInfoResponse>))]
    [HttpGet(Name = nameof(ReadChat))]
    public async Task<IActionResult> ReadChat([FromRoute] Guid chatId)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var chatDto = new ReadSingleChatShortInfoResponse(chatResult.Value);

        return Ok(chatDto);
    }

    [HttpPatch(Name = nameof(ChangeChatInfo))]
    public async Task<IActionResult> ChangeChatInfo([FromRoute] Guid chatId)
    {
        // Todo: Implement changing chat info
        throw new NotImplementedException();
    }

    [HttpGet("join", Name = nameof(JoinChat))]
    public async Task<IActionResult> JoinChat([FromRoute] Guid chatId)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var joinResult = await _chatMembershipService.JoinUserToChat(userProfile, chat);
        if (joinResult.IsFailed)
        {
            return Failure(joinResult.Errors);
        }

        await _chatPermissionsService.EnsureUserHasPermissionInChat(chat, userProfile, MembershipRight.Write);
        await _systemMessagesService.CreateJoinedMessage(chat, userProfile);

        return Ok();
    }

    [HttpGet("leave", Name = nameof(LeaveChat))]
    public async Task<IActionResult> LeaveChat([FromRoute] Guid chatId)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var leaveResult = await _chatMembershipService.JoinUserToChat(userProfile, chat);
        if (leaveResult.IsFailed)
        {
            return Failure(leaveResult.Errors);
        }

        await _systemMessagesService.CreateLeftMessage(chat, userProfile);

        return Ok();
    }
}