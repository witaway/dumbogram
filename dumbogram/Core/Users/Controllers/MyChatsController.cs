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
[Route("/api/users/me/chats")]
[ApiController]
public class MyChatsController : ApplicationController
{
    private readonly ChatService _chatService;

    private readonly ILogger<ChatsController> _logger;
    private readonly UserService _userService;

    public MyChatsController(
        ChatService chatService,
        UserService userService,
        ILogger<ChatsController> logger
    )
    {
        _chatService = chatService;
        _userService = userService;
        _logger = logger;
    }

    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(ResponseSuccess<ReadMultipleChatsShortInfoResponseDto>)
    )]
    [HttpGet("owned")]
    public async Task<IActionResult> ReadOwnedChats()
    {
        var userProfile = await _userService.ReadUserProfileById(User.GetApplicationUserId());

        var chats = await _chatService.ReadAllChatsOwnedBy(userProfile!);

        var chatsDto = new ReadMultipleChatsShortInfoResponseDto(chats);
        return Ok(chatsDto);
    }

    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(ResponseSuccess<ReadMultipleChatsShortInfoResponseDto>)
    )]
    [HttpGet("joined")]
    public async Task<IActionResult> ReadJoinedChats()
    {
        var userProfile = await _userService.ReadUserProfileById(User.GetApplicationUserId());

        var chats = await _chatService.ReadAllChatsJoinedBy(userProfile!);

        var chatsDto = new ReadMultipleChatsShortInfoResponseDto(chats);
        return Ok(chatsDto);
    }
}