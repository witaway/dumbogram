using System.Runtime.CompilerServices;
using Dumbogram.Application.Chats.Controllers.Dto;
using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Messages.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Database.KeysetPagination;
using Dumbogram.Database.KeysetPagination.Dto;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Dto;
using Dumbogram.Models.Base;
using Dumbogram.Models.Chats;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dumbogram.Application.Chats.Controllers;


[Authorize]
[Route("/api/chats", Name = "Chats")]
[ApiController]
public class ChatsController : ApplicationController
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatPermissionsService _chatPermissionsService;
    private readonly ChatService _chatService;
    private readonly ILogger<ChatsController> _logger;
    private readonly SystemMessagesService _systemMessagesService;
    private readonly UserResolverService _userResolverService;

    public ChatsController(
        ChatService chatService,
        ChatMembershipService chatMembershipService,
        SystemMessagesService systemMessagesService,
        UserResolverService userResolverService,
        ChatPermissionsService chatPermissionsService,
        ILogger<ChatsController> logger
    )
    {
        _chatService = chatService;
        _chatMembershipService = chatMembershipService;
        _userResolverService = userResolverService;
        _chatPermissionsService = chatPermissionsService;
        _systemMessagesService = systemMessagesService;
        _logger = logger;
    }

    [ProducesResponseType(
        StatusCodes.Status200OK, Type = typeof(ResponseSuccess<ReadMultipleChatsShortInfoResponse>)
    )] 
    [HttpGet("search", Name = nameof(ReadAllChats))]
    public async Task<IActionResult> ReadAllChats([FromQuery] PagingQuery pagingQuery)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var keysetParsingStrategy = new PagingQueryKeysetParsingByOrderNameStrategy<Chat>()
            .WithName("latest", new Keyset<Chat>()
                .Descending(m => m.CreatedDate)
                .Ascending(m => m.Id)
            )
            .WithName("oldest", new Keyset<Chat>()
                .Ascending(m => m.CreatedDate)
                .Ascending(m => m.Id)
            );
        
        var pagingParser = new PagingQueryParser<Chat>(keysetParsingStrategy, 50);
        var pagingDetails = pagingParser.GetPagingDetails(pagingQuery);
        
        var chats = await _chatService.ReadAllPublicOrAccessibleChats(userProfile!, pagingDetails);
        
        return Ok(new
        {
            Chats = chats.Select(chat => new ReadSingleChatShortInfoResponse(chat)),
            NextPageToken = chats.NextPageToken,
            PrevPageToken = chats.PrevPageToken,
            chats.Total,
            chats.Count
        });
        //var chatsDto = new ReadMultipleChatsShortInfoResponse(chats);
        //return Ok(chatsDto);
    }

    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseSuccess))]
    [HttpPost(Name = nameof(CreateChat))]
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
        await _chatMembershipService.EnsureUserJoinedInChat(userProfile, chat);
        await _chatPermissionsService.EnsureUserHasPermissionInChat(chat, userProfile, MembershipRight.Owner);
        await _systemMessagesService.CreateChatCreatedMessage(chat);

        var chatUri = $"/api/chats/{chat.Id}";
        return Created(chatUri, null);
    }
}