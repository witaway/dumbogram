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
[Route("/api/chats")]
[ApiController]
public class ChatsController : ApplicationController
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly UserResolverService _userResolverService;
    private readonly UserService _userService;

    public ChatsController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        UserService userService,
        UserResolverService userResolverService,
        ILogger<ChatsController> logger
    )
    {
        _chatService = chatService;
        _userService = userService;
        _chatMembershipService = chatMembershipService;
        _userResolverService = userResolverService;
        _logger = logger;
    }

    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadMultipleChatsShortInfoResponse>)
    )]
    [HttpGet("search")]
    public async Task<IActionResult> ReadAllChats()
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chats = await _chatService.ReadAllPublicOrAccessibleChats(userProfile!);
        var chatsDto = new ReadMultipleChatsShortInfoResponse(chats);

        return Ok(chatsDto);
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseSuccess))]
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest dto)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chat = new Chat
        {
            OwnerProfile = userProfile!,
            Description = dto.Description,
            Title = dto.Title
        };

        await _chatService.CreateChat(chat);
        await _chatMembershipService.EnsureUserJoinedInChat(userProfile!, chat);

        var chatUri = $"/api/chats/{chat.Id}";
        return Created(chatUri, null);
    }
}