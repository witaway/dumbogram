using Dumbogram.Common.Extensions;
using Dumbogram.Core.Chats.Dto;
using Dumbogram.Core.Chats.Errors;
using Dumbogram.Core.Chats.Services;
using Dumbogram.Core.Users.Errors;
using Dumbogram.Core.Users.Services;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}/members/{memberId:guid}")]
[ApiController]
public class ChatMemberRightsController : ControllerBase
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatPermissionsService _chatPermissionsService;
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly UserService _userService;

    public ChatMemberRightsController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        ChatPermissionsService chatPermissionsService,
        UserService userService,
        ILogger<ChatsController> logger
    )
    {
        _chatService = chatService;
        _userService = userService;
        _chatMembershipService = chatMembershipService;
        _chatPermissionsService = chatPermissionsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetRightsOfMember(Guid chatId, Guid memberId)
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
        var isOwner = _chatPermissionsService.IsUserOwnerOfChat(chat, userProfile!);

        if (!isOwner)
        {
            var result = Result.Fail(new NotEnoughPermissionsError("Not enough permissions"));
            return StatusCode(StatusCodes.Status403Forbidden, result.ToFailureDto("Not enough permissions"));
        }

        var memberProfileResult = await _userService.RequestUserProfileById(memberId);

        if (memberProfileResult.IsFailed)
        {
            if (memberProfileResult.HasError<UserNotFoundError>())
            {
                return NotFound(memberProfileResult.ToFailureDto("Member not found"));
            }

            return BadRequest(memberProfileResult.ToFailureDto("Cannot access member"));
        }

        var memberProfile = memberProfileResult.Value;

        var rights = await _chatPermissionsService.ReadAllRightsAppliedToUsersInChat(chat, memberProfile);
        var rightsDto = new ReadMultipleRightsResponseDto(rights);

        return Ok(Common.Dto.Response.Success("Rights got successfully", rightsDto));
    }

    [HttpPut]
    public async Task<IActionResult> ApplyRightsToMember(
        Guid chatId,
        Guid memberId,
        [FromBody] ApplyRightsRequestDto dto)
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
        var isOwner = _chatPermissionsService.IsUserOwnerOfChat(chat, userProfile!);

        if (!isOwner)
        {
            var result = Result.Fail(new NotEnoughPermissionsError("Not enough permissions"));
            return StatusCode(StatusCodes.Status403Forbidden, result.ToFailureDto("Not enough permissions"));
        }

        var memberProfileResult = await _userService.RequestUserProfileById(memberId);

        if (memberProfileResult.IsFailed)
        {
            if (memberProfileResult.HasError<UserNotFoundError>())
            {
                return NotFound(memberProfileResult.ToFailureDto("Member not found"));
            }

            return BadRequest(memberProfileResult.ToFailureDto("Cannot access member"));
        }

        var memberProfile = memberProfileResult.Value;
        var isMemberOwner = _chatPermissionsService.IsUserOwnerOfChat(chat, memberProfile);

        if (isMemberOwner)
        {
            var result = Result.Fail(new CannotChangeOwnerRights("Cannot change owner rights"));
            return BadRequest(result.ToFailureDto("Cannot change owner rights"));
        }

        var rights = dto.ConvertToRightsList();

        await _chatPermissionsService.EnsureRightsAppliedToUserInChat(chat, memberProfile, rights);

        return Ok();
    }
}