using Dumbogram.Api.Application.UseCases.Chat.Commands.JoinChat;
using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Messages;
using Dumbogram.Api.Domain.Services.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chat.Commands.LeaveChat;

public class LeaveChatHandler(
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
        var joinResult = await chatMembershipService.LeaveUserFromChat(currentUser, chat);
        if (joinResult.IsFailed) return Result.Fail(joinResult.Errors);

        await systemMessagesService.CreateLeftMessage(chat, currentUser);

        return Result.Ok();
    }
}