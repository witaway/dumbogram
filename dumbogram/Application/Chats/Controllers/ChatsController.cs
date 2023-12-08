using System.Runtime.CompilerServices;
using Dumbogram.Application.Chats.Controllers.Dto;
using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Messages.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Database.KeysetPagination;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Dto;
using Dumbogram.Models.Base;
using Dumbogram.Models.Chats;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dumbogram.Application.Chats.Controllers;

public abstract class PagingQueryBase
{
    [BindRequired]
    [FromQuery(Name = "take")]
    public int Take { get; set; }

    [BindRequired]
    [FromQuery(Name = "order")]
    public string Order { get; set; } = null!;

    [FromQuery(Name = "first")]
    public bool First { get; set; }

    [FromQuery(Name = "last")]
    public bool Last { get; set; }

    [FromQuery(Name = "prev_page_token")]
    public string? PrevPageToken { get; set; }

    [FromQuery(Name = "next_page_token")]
    public string? NextPageToken { get; set; }

    public Cursor<TEntity> GetCursor<TEntity>() where TEntity : BaseEntity
    {
        if (First == false && Last == false &&
            PrevPageToken == null && NextPageToken == null)
        {
            First = true;
        }

        if (First)
        {
            return Cursor<TEntity>.First(Take);
        }

        if (Last)
        {
            return Cursor<TEntity>.Last(Take);
        }

        if (PrevPageToken != null)
        {
            return Cursor<TEntity>.Decode(GetKeyset<TEntity>(), PrevPageToken, PaginationDirection.Backward, Take);
        }

        if (NextPageToken != null)
        {
            return Cursor<TEntity>.Decode(GetKeyset<TEntity>(), NextPageToken, PaginationDirection.Forward, Take);
        }

        throw new SwitchExpressionException();
    }

    public abstract Keyset<TEntity> GetKeyset<TEntity>() where TEntity : BaseEntity;
}

public class PagingQueryBaseValidator<TEntity> : AbstractValidator<PagingQueryBase> where TEntity : BaseEntity
{
    public PagingQueryBaseValidator()
    {
        RuleFor(q => q)
            .Must(q => OptionalsSpecifiedCount(q) <= 0)
            .WithMessage("Only one of first, last, prev_page_token, next_page_token is allowed");
    }

    private static int OptionalsSpecifiedCount(PagingQueryBase q)
    {
        var count = 0;
        if (q.First) count++;
        if (q.Last) count++;
        if (q.NextPageToken != null) count++;
        if (q.PrevPageToken != null) count++;
        return count;
    }
}

public class ChatsPagingQuery : PagingQueryBase
{
    public override Keyset<Chat> GetKeyset<Chat>()
    {
        return Order switch
        {
            "latest" => new Keyset<Chat>()
                .Descending(m => m.CreatedDate)
                .Ascending(m => m.Id),

            "oldest" => new Keyset<Chat>()
                .Ascending(m => m.CreatedDate)
                .Ascending(m => m.Id),

            _ => throw new InvalidCastException("Order is not valid")
        };
    }
}

public class ChatsPagingQueryValidator : PagingQueryBaseValidator<Chat>
{
}

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
    public async Task<IActionResult> ReadAllChats([FromQuery] ChatsPagingQuery pagingQuery)
    {
        var userProfile = await _userResolverService.GetApplicationUser();

        var chats = await _chatService.ReadAllPublicOrAccessibleChats(userProfile!, pagingQuery.GetKeyset(),
            pagingQuery.GetCursor());
        return Ok(new
        {
            Chats = chats.Select(chat => new ReadSingleChatShortInfoResponse(chat)),
            NextPageToken = chats.Forward,
            PrevPageToken = chats.Backward,
            chats.Total,
            chats.Count
        });
        // var chatsDto = new ReadMultipleChatsShortInfoResponse(chats);

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