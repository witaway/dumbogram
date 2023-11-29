using Dumbogram.Application.Chats.Controllers;
using Dumbogram.Application.Chats.Controllers.Dto;
using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Users.Controllers;

[Authorize]
[Route("/api/users/me/chats")]
[ApiController]
public class MyChatsController : ApplicationController
{
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly UserResolverService _userResolverService;

    public MyChatsController(
        ChatService chatService,
        UserResolverService userResolverService,
        ILogger<ChatsController> logger
    )
    {
        _chatService = chatService;
        _userResolverService = userResolverService;
        _logger = logger;
    }

    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(ResponseSuccess<ReadMultipleChatsShortInfoResponse>)
    )]
    [HttpGet("owned")]
    public async Task<IActionResult> ReadOwnedChats()
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chats = await _chatService.ReadAllChatsOwnedBy(userProfile!);
        var chatsDto = new ReadMultipleChatsShortInfoResponse(chats);

        return Ok(chatsDto);
    }

    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(ResponseSuccess<ReadMultipleChatsShortInfoResponse>)
    )]
    [HttpGet("joined")]
    public async Task<IActionResult> ReadJoinedChats()
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chats = await _chatService.ReadAllChatsJoinedBy(userProfile!);
        var chatsDto = new ReadMultipleChatsShortInfoResponse(chats);

        return Ok(chatsDto);
    }
}