using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Messages;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMessage.Queries.GetChatMessage;

public class GetChatMessageHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    MessagesService messagesService
) : IRequestHandler<GetChatMessageRequest, Result<Message>>
{
    public async Task<Result<Message>> Handle(
        GetChatMessageRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var subjectUser = await userResolverService.GetApplicationUser();
        var chatId = request.ChatId;

        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed) return Result.Fail(chatResult.Errors);
        var chat = chatResult.Value;

        var messageId = request.MessageId;
        var messageResult = await messagesService.QuerySingleMessageById(subjectUser, chat, messageId);
        if (messageResult.IsFailed) return Result.Fail(messageResult.Errors);

        var message = messageResult.Value;
        return Result.Ok(message);
    }
}