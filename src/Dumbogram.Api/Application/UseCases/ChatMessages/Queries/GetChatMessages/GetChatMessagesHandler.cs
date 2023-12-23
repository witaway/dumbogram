using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Messages;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMessages.Queries.GetChatMessages;

public class GetChatMessagesHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    MessagesService messagesService
) : IRequestHandler<GetChatMessagesRequest, Result<IEnumerable<Message>>>
{
    public async Task<Result<IEnumerable<Message>>> Handle(
        GetChatMessagesRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();

        // Retrieve chat
        var chatId = request.ChatId;
        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(chatId, currentUser);
        if (chatResult.IsFailed) return Result.Fail(chatResult.Errors);
        var chat = chatResult.Value;

        // Retrieve messages
        var messagesResult = await messagesService.QueryManyMessages(currentUser, chat);
        if (messagesResult.IsFailed) return Result.Fail(messagesResult.Errors);
        var messages = messagesResult.Value;

        return Result.Ok(messages);
    }
}