using Dumbogram.Common.Helpers;
using Dumbogram.Core.Chats.Dto;
using Dumbogram.Core.Chats.Models;
using Dumbogram.Core.Chats.Services;
using Dumbogram.Core.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Core.Chats.Controllers;

[Authorize]
[Route("/api/[controller]")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatPermissionsService _chatPermissionsService;

    private readonly ChatService _chatService;

    private readonly ILogger<ChatsController> _logger;
    private readonly UserService _userService;

    public ChatsController(
        ChatService chatService,
        IdentityUserService identityUserService,
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

    [HttpGet]
    public IActionResult ReadAllChats()
    {
        var chats = _chatService.ReadAllChats();
        return Ok(chats);
    }

    [HttpGet]
    [Route("{chatId:guid}")]
    public IActionResult ReadChatByChatId([FromRoute] Guid chatId)
    {
        var chat = _chatService.ReadChatById(chatId);
        return Ok(chat);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequestDto dto)
    {
        // Read current user
        var uid = User.GetUserApplicationId();
        var userProfile = await _userService.ReadUserProfileById(uid);

        // Chat entity instance
        var chat = new Chat
        {
            OwnerProfile = userProfile,
            Description = dto.Description,
            Title = dto.Title
        };

        _chatService.CreateChat(chat);
        _chatMembershipService.JoinUser(userProfile!, chat);

        return Ok();
    }
}