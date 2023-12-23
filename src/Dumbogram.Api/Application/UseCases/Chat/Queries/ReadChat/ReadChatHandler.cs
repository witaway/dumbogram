using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chat.Queries.ReadChat;

public class ReadChatHandler(
    UserResolverService userResolverService,
    ChatService chatService
)
    : IRequestHandler<ReadChatRequest, Result<Persistence.Context.Application.Entities.Chats.Chat>>
{
    public async Task<Result<Persistence.Context.Application.Entities.Chats.Chat>> Handle(
        ReadChatRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(request.ChatId, currentUser);
        return chatResult;
    }
}