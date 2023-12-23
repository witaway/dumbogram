using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Messages;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chat.Commands.CreateChat;

public class CreateChatHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    ChatMembershipService chatMembershipService,
    ChatPermissionsService chatPermissionsService,
    SystemMessagesService systemMessagesService
) : IRequestHandler<CreateChatRequest, Result<Persistence.Context.Application.Entities.Chats.Chat>>
{
    public async Task<Result<Persistence.Context.Application.Entities.Chats.Chat>> Handle(
        CreateChatRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();

        var chat = new Persistence.Context.Application.Entities.Chats.Chat
        {
            OwnerProfile = currentUser!,
            Title = request.Title,
            Description = request.Description
        };

        await chatService.CreateChat(chat);
        await chatMembershipService.EnsureUserJoinedInChat(currentUser, chat);
        await chatPermissionsService.EnsureUserHasPermissionInChat(chat, currentUser, MembershipRight.Owner);
        await systemMessagesService.CreateChatCreatedMessage(chat);

        return Result.Ok(chat);
    }
}