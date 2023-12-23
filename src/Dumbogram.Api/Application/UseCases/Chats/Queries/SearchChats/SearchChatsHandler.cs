using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chats.Queries.SearchChats;

public class SearchChatsHandler(
    ChatService chatService,
    UserResolverService userResolverService
) : IRequestHandler<SearchChatsRequest, Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>>
{
    public async Task<Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>> Handle(
        SearchChatsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var chats = await chatService.ReadAllPublicOrAccessibleChats(currentUser);
        return Result.Ok(chats);
    }
}