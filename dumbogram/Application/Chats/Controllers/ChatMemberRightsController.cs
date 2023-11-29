﻿using Dumbogram.Application.Chats.Dto;
using Dumbogram.Application.Chats.Errors;
using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Infrasctructure.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Chats.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}/members/{memberId:guid}")]
[ApiController]
public class ChatMemberRightsController : ApplicationController
{
    private readonly ChatPermissionsService _chatPermissionsService;
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly UserResolverService _userResolverService;
    private readonly UserService _userService;

    public ChatMemberRightsController(
        ChatService chatService,
        ChatPermissionsService chatPermissionsService,
        UserService userService,
        ILogger<ChatsController> logger,
        UserResolverService userResolverService
    )
    {
        _chatService = chatService;
        _userService = userService;
        _chatPermissionsService = chatPermissionsService;
        _userResolverService = userResolverService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetRightsOfMember(Guid chatId, Guid memberId)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;

        var isOwner = _chatPermissionsService.IsUserOwnerOfChat(chat, userProfile!);
        if (!isOwner)
        {
            return Failure(new NotEnoughRightsError());
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
        var userProfile = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, userProfile!);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;
        var isOwner = _chatPermissionsService.IsUserOwnerOfChat(chat, userProfile!);
        if (!isOwner)
        {
            return Failure(new NotEnoughRightsError());
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