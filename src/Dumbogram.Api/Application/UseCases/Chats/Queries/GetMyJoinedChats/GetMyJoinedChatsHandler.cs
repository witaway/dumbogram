using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.Chats.Queries.GetMyJoinedChats;

public class GetMyJoinedChatsHandler(
    UserResolverService userResolverService,
    ChatService chatService
) : IRequestHandler<GetMyJoinedChatsRequest, Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>>
{
    public async Task<Result<IEnumerable<Persistence.Context.Application.Entities.Chats.Chat>>> Handle(
        GetMyJoinedChatsRequest request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();

        var chats = await chatService.ReadAllChatsJoinedBy(currentUser);

        return Result.Ok(chats);
    }
}