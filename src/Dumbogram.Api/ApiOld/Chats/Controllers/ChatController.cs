using Dumbogram.Api.ApiOld.Chats.Controllers.Dto;
using Dumbogram.Api.ApiOld.Chats.Services;
using Dumbogram.Api.ApiOld.Messages.Services;
using Dumbogram.Api.ApiOld.Users.Services;
using Dumbogram.Api.Infrasctructure.Controller;
using Dumbogram.Api.Infrasctructure.Dto;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.ApiOld.Chats.Controllers;

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
        if (chatResult.IsFailed) return Failure(chatResult.Errors);

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
        if (chatResult.IsFailed) return Failure(chatResult.Errors);

        var chat = chatResult.Value;
        var joinResult = await _chatMembershipService.JoinUserToChat(userProfile, chat);
        if (joinResult.IsFailed) return Failure(joinResult.Errors);

        await _chatPermissionsService.EnsureUserHasPermissionInChat(chat, userProfile, MembershipRight.Write);
        await _systemMessagesService.CreateJoinedMessage(chat, userProfile);

        return Ok();
    }

    [HttpGet("leave", Name = nameof(LeaveChat))]
    public async Task<IActionResult> LeaveChat([FromRoute] Guid chatId)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile);
        if (chatResult.IsFailed) return Failure(chatResult.Errors);

        var chat = chatResult.Value;
        var leaveResult = await _chatMembershipService.JoinUserToChat(userProfile, chat);
        if (leaveResult.IsFailed) return Failure(leaveResult.Errors);

        await _systemMessagesService.CreateLeftMessage(chat, userProfile);

        return Ok();
    }
}