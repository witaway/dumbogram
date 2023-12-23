using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Messages;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chat.Commands.JoinChat;

public class JoinChatHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    ChatMembershipService chatMembershipService,
    ChatPermissionsService chatPermissionsService,
    SystemMessagesService systemMessagesService
) : IRequestHandler<JoinChatRequest, Result>
{
    public async Task<Result> Handle(JoinChatRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var chatId = request.ChatId;

        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(chatId, currentUser);
        if (chatResult.IsFailed) return Result.Fail(chatResult.Errors);

        var chat = chatResult.Value;
        var joinResult = await chatMembershipService.JoinUserToChat(currentUser, chat);
        if (joinResult.IsFailed) return Result.Fail(joinResult.Errors);

        await chatPermissionsService.EnsureUserHasPermissionInChat(chat, currentUser, MembershipRight.Write);
        await systemMessagesService.CreateJoinedMessage(chat, currentUser);

        return Result.Ok();
    }
}