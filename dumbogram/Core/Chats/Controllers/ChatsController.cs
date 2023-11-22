using Dumbogram.Common.Dto;
using Dumbogram.Common.Extensions;
using Dumbogram.Core.Chats.Dto;
using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Chats.Services;
using Dumbogram.Core.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Chats.Controllers;

[Authorize]
[Route("/api/chats")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly ChatMembershipService _chatMembershipService;

    private readonly ChatService _chatService;

    private readonly ILogger<ChatsController> _logger;
    private readonly UserService _userService;

    public ChatsController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        UserService userService,
        ILogger<ChatsController> logger
    )
    {
        _chatService = chatService;
        _userService = userService;
        _chatMembershipService = chatMembershipService;
        _logger = logger;
    }

    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadMultipleChatsShortInfoResponseDto>)
    )]
    [HttpGet("search")]
    public async Task<IActionResult> ReadAllChats()
    {
        var userProfile = await _userService.ReadUserProfileById(User.GetUserApplicationId());

        var chats = await _chatService.ReadAllPublicOrAccessibleChats(userProfile!);

        var chatsDto = new ReadMultipleChatsShortInfoResponseDto(chats);
        return Ok(Common.Dto.Response.Success("Chats list accessed successfully", chatsDto));
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseSuccess))]
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequestDto dto)
    {
        var uid = User.GetUserApplicationId();
        var userProfile = await _userService.ReadUserProfileById(uid);

        var chat = new Chat
        {
            OwnerProfile = userProfile!,
            Description = dto.Description,
            Title = dto.Title
        };

        await _chatService.CreateChat(chat);
        await _chatMembershipService.EnsureUserJoinedInChat(userProfile!, chat);

        var chatUri = $"/api/chats/{chat.Id}";

        return Created(chatUri, Common.Dto.Response.Success("Chat created successfully"));
    }
}