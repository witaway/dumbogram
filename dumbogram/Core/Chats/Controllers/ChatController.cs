using Dumbogram.Common.Dto;
using Dumbogram.Common.Extensions;
using Dumbogram.Core.Chats.Dto;
using Dumbogram.Core.Chats.Errors;
using Dumbogram.Core.Chats.Services;
using Dumbogram.Core.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}")]
[ApiController]
public class ChatController : ControllerBase
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
            if (chatResult.HasError<ChatNotFoundError>())
            {
                return NotFound(chatResult.ToFailureDto("Chat not found"));
            }

            return BadRequest(chatResult.ToFailureDto("Cannot access chat"));
        }

        var chat = chatResult.Value;
        var chatDto = new ReadSingleChatShortInfoResponseDto(chat);
        return Ok(Common.Dto.Response.Success("Chat found successfully", chatDto));
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
            if (chatResult.HasError<ChatNotFoundError>())
            {
                return NotFound(chatResult.ToFailureDto("Chat not found"));
            }

            return BadRequest(chatResult.ToFailureDto("Cannot access chat"));
        }

        var chat = chatResult.Value;
        var joinResult = await _chatMembershipService.JoinUserToChat(userProfile!, chat);

        if (joinResult.IsFailed)
        {
            if (joinResult.HasError<UserBannedInChatError>())
            {
                return StatusCode(StatusCodes.Status403Forbidden, joinResult.ToFailureDto("User banned in chat"));
            }

            if (joinResult.HasError<UserAlreadyJoinedToChatError>())
            {
                return Conflict(joinResult.ToFailureDto("User already joined to chat"));
            }

            return BadRequest(joinResult.ToFailureDto("Cannot join chat"));
        }

        return Ok(Common.Dto.Response.Success("Joined chat successfully"));
    }

    [HttpGet("leave")]
    public async Task<IActionResult> LeaveChat([FromRoute] Guid chatId)
    {
        var uid = User.GetApplicationUserId();
        var userProfile = await _userService.ReadUserProfileById(uid);

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);

        if (chatResult.IsFailed)
        {
            if (chatResult.HasError<ChatNotFoundError>())
            {
                return NotFound(chatResult.ToFailureDto("Chat not found"));
            }

            return BadRequest(chatResult.ToFailureDto("Cannot access chat"));
        }

        var chat = chatResult.Value;

        var leaveResult = await _chatMembershipService.JoinUserToChat(userProfile!, chat!);

        if (leaveResult.IsFailed)
        {
            if (leaveResult.HasError<UserAlreadyLeftFromChatError>())
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    leaveResult.ToFailureDto("User already left from chat"));
            }

            return BadRequest(leaveResult.ToFailureDto("Cannot left chat"));
        }

        return Ok(Common.Dto.Response.Success("Left chat successfully"));
    }
}