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
    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<List<ReadChatByChatIdResponseDto>>)
    )]
    public async Task<IActionResult> ReadAllChats()
    {
        var chats = await _chatService.ReadAllChats();
        var chatsDto = chats.Select(ReadChatByChatIdResponseDto.MapFromModel).ToList();
        return Ok(Common.Dto.Response.Success("Chats list accessed successfully", chatsDto));
    }

    [HttpGet]
    [Route("{chatId:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadChatByChatIdResponseDto>))]
    public async Task<IActionResult> ReadChatByChatId([FromRoute] Guid chatId)
    {
        var chat = await _chatService.ReadChatById(chatId);
        if (chat == null)
        {
            return NotFound(Common.Dto.Response.Failure("Chat not found"));
        }

        var chatDto = ReadChatByChatIdResponseDto.MapFromModel(chat);
        return Ok(Common.Dto.Response.Success("Chat was found successfully", chatDto));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseSuccess))]
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

        await _chatService.CreateChat(chat);
        await _chatMembershipService.JoinUser(userProfile!, chat);

        var chatUri = $"/api/chats/{chat.Id}";

        return Created(chatUri, Common.Dto.Response.Success("Chat created successfully"));
    }
}