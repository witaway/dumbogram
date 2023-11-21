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
[Route("/api/chats/public")]
[ApiController]
public class PublicChatsController : ControllerBase
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatPermissionsService _chatPermissionsService;

    private readonly ChatService _chatService;

    private readonly ILogger<PublicChatsController> _logger;
    private readonly UserService _userService;

    public PublicChatsController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        ChatPermissionsService chatPermissionsService,
        UserService userService,
        ILogger<PublicChatsController> logger
    )
    {
        _chatService = chatService;
        _chatPermissionsService = chatPermissionsService;
        _userService = userService;
        _chatMembershipService = chatMembershipService;
        _logger = logger;
    }

    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadMultipleChatsResponseDto>)
    )]
    [HttpGet]
    public async Task<IActionResult> ReadAllChats()
    {
        var userProfile = await _userService.ReadUserProfileById(User.GetUserApplicationId());

        var chats = await _chatService.ReadAllPublicOrAccessibleChats(userProfile!);

        var chatsDto = new ReadMultipleChatsResponseDto(chats);
        return Ok(Common.Dto.Response.Success("Chats list accessed successfully", chatsDto));
    }

    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseFailure))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadSingleChatByChatIdResponseDto>))]
    [HttpGet("{chatId:guid}")]
    public async Task<IActionResult> ReadChatByChatId([FromRoute] Guid chatId)
    {
        var userProfile = await _userService.ReadUserProfileById(User.GetUserApplicationId());

        var chat = await _chatService.ReadPublicOrAccessibleChatByChatId(chatId, userProfile!);
        if (chat == null)
        {
            return NotFound(Common.Dto.Response.Failure("Chat not found"));
        }

        var chatDto = new ReadSingleChatByChatIdResponseDto(chat);
        return Ok(Common.Dto.Response.Success("Chat was found successfully", chat));
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseSuccess))]
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatRequestDto dto)
    {
        var uid = User.GetUserApplicationId();
        var userProfile = await _userService.ReadUserProfileById(uid);

        var chat = new Chat
        {
            OwnerProfile = userProfile,
            Description = dto.Description,
            Title = dto.Title
        };

        await _chatService.CreateChat(chat);
        await _chatMembershipService.EnsureUserJoinedInChat(userProfile!, chat);

        var chatUri = $"/api/chats/{chat.Id}";

        return Created(chatUri, Common.Dto.Response.Success("Chat created successfully"));
    }

    [HttpGet("{chatId:guid}/join")]
    public async Task<IActionResult> JoinChat([FromRoute] Guid chatId)
    {
        var uid = User.GetUserApplicationId();
        var userProfile = await _userService.ReadUserProfileById(uid);

        var chat = await _chatService.ReadPublicChatByChatId(chatId);

        await _chatMembershipService.EnsureUserJoinedInChat(userProfile, chat);

        return Ok(Common.Dto.Response.Success("Chat joined successfully"));
    }
}