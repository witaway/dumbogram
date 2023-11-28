using Dumbogram.Application.Chats.Dto;
using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}")]
[ApiController]
public class ChatController : ApplicationController
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly UserResolverService _userResolverService;

    public ChatController(
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

    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadSingleChatShortInfoResponseDto>))]
    [HttpGet]
    public async Task<IActionResult> ReadChat([FromRoute] Guid chatId)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

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

        return Ok();
    }

    [HttpGet("leave")]
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

        return Ok();
    }
}