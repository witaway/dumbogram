using Dumbogram.Common.Controller;
using Dumbogram.Common.Extensions;
using Dumbogram.Core.Chats.Dto;
using Dumbogram.Core.Chats.Errors;
using Dumbogram.Core.Chats.Services;
using Dumbogram.Core.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}/members/{memberId:guid}")]
[ApiController]
public class ChatMemberRightsController : ApplicationController
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
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var isOwner = _chatPermissionsService.IsUserOwnerOfChat(chat, userProfile!);

        if (!isOwner)
        {
            return Failure(new NotEnoughPermissionsError());
        }

        var memberProfileResult = await _userService.RequestUserProfileById(memberId);

        if (memberProfileResult.IsFailed)
        {
            return Failure(memberProfileResult.Errors);
        }

        var memberProfile = memberProfileResult.Value;

        var rights = await _chatPermissionsService.ReadAllRightsAppliedToUsersInChat(chat, memberProfile);
        var rightsDto = new ReadMultipleRightsResponseDto(rights);

        return Ok(rightsDto);
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
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var isOwner = _chatPermissionsService.IsUserOwnerOfChat(chat, userProfile!);

        if (!isOwner)
        {
            return Failure(new NotEnoughPermissionsError());
        }

        var memberProfileResult = await _userService.RequestUserProfileById(memberId);

        if (memberProfileResult.IsFailed)
        {
            return Failure(memberProfileResult.Errors);
        }

        var memberProfile = memberProfileResult.Value;
        var isMemberOwner = _chatPermissionsService.IsUserOwnerOfChat(chat, memberProfile);

        if (isMemberOwner)
        {
            return Failure(new CannotChangeOwnerRights());
        }

        var rights = dto.ConvertToRightsList();

        await _chatPermissionsService.EnsureRightsAppliedToUserInChat(chat, memberProfile, rights);

        return Ok();
    }
}