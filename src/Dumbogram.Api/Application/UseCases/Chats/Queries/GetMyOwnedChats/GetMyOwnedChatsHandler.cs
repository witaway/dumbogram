using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chats.Queries.GetMyOwnedChats;

public class GetMyOwnedChatsHandler(
    UserResolverService userResolverService,
    ChatService chatService
) : IRequestHandler<GetMyOwnedChatsRequest, Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>>
{
    public async Task<Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>> Handle(
        GetMyOwnedChatsRequest request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();

        var chats = await chatService.ReadAllChatsOwnedBy(currentUser);

        return Result.Ok(chats);
    }
}