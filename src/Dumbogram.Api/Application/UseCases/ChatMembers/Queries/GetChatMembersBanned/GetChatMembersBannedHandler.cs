using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMembers.Queries.GetChatMembersBanned;

public class GetChatMembersBannedHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    ChatMembershipService chatMembershipService
) : IRequestHandler<GetChatMembersBannedRequest, Result<IEnumerable<UserProfile>>>
{
    public async Task<Result<IEnumerable<UserProfile>>> Handle(
        GetChatMembersBannedRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var chatId = request.ChatId;

        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(chatId, currentUser);
        if (chatResult.IsFailed) return Result.Fail(chatResult.Errors);
        var chat = chatResult.Value;

        var members = await chatMembershipService.ReadAllChatBannedUsers(chat);
        return Result.Ok(members);
    }
}