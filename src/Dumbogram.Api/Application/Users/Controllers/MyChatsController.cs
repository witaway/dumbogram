using Dumbogram.Api.Application.Chats.Controllers;
using Dumbogram.Api.Application.Chats.Controllers.Dto;
using Dumbogram.Api.Application.Chats.Services;
using Dumbogram.Api.Application.Users.Services;
using Dumbogram.Api.Infrasctructure.Controller;
using Dumbogram.Api.Infrasctructure.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Application.Users.Controllers;

[Authorize]
[Route("/api/users/me/chats", Name = "My chats")]
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
    [HttpGet("owned", Name = nameof(ReadOwnedChats))]
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
    [HttpGet("joined", Name = nameof(ReadJoinedChats))]
    public async Task<IActionResult> ReadJoinedChats()
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chats = await _chatService.ReadAllChatsJoinedBy(userProfile!);
        var chatsDto = new ReadMultipleChatsShortInfoResponse(chats);

        return Ok(chatsDto);
    }
}