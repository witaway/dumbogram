using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Messages;
using Dumbogram.Api.Domain.Services.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMessage.Commands.DeleteChatMessage;

public class DeleteChatMessageHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    MessagesService messagesService
) : IRequestHandler<DeleteChatMessageRequest, Result>
{
    public async Task<Result> Handle(DeleteChatMessageRequest request, CancellationToken cancellationToken)
    {
        var subjectUser = await userResolverService.GetApplicationUser();
        var chatId = request.ChatId;
        var messageId = request.MessageId;

        // Retrieve chat
        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed) return Result.Fail(chatResult.Errors);
        var chat = chatResult.Value;

        // Retrieve message
        var messageResult = await messagesService.QuerySingleMessageById(subjectUser, chat, messageId);
        if (messageResult.IsFailed) return Result.Fail(messageResult.Errors);
        var message = messageResult.Value;

        // Try delete message
        var deleteMessageResult = await messagesService.DeleteMessage(subjectUser, message);
        if (deleteMessageResult.IsFailed) return Result.Fail(deleteMessageResult.Errors);

        return Result.Ok();
    }
}