using Dumbogram.Api.Application.Errors.Chats;
using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMember.Queries.GetChatMemberRights;

public class GetChatMemberRightsHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    UserService userService,
    ChatPermissionsService chatPermissionsService
) : IRequestHandler<GetChatMemberRightsRequest, Result<IEnumerable<MembershipRight>>>
{
    public async Task<Result<IEnumerable<MembershipRight>>> Handle(
        GetChatMemberRightsRequest request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var chatId = request.ChatId;
        var memberId = request.MemberId;

        // Retrieve chat
        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(chatId, currentUser!);
        if (chatResult.IsFailed) return Result.Fail(chatResult.Errors);
        var chat = chatResult.Value;

        // Check rights to retrieve permissions list 
        var isOwner = chatPermissionsService.IsUserOwnerOfChat(chat, currentUser!);
        if (!isOwner) return Result.Fail(new NotEnoughRightsError());

        // Retrieve needed user
        var memberProfileResult = await userService.RequestUserProfileById(memberId);
        if (memberProfileResult.IsFailed) return Result.Fail(memberProfileResult.Errors);
        var memberProfile = memberProfileResult.Value;

        // Retrieve rights
        var rights = await chatPermissionsService.ReadAllRightsAppliedToUsersInChat(chat, memberProfile);

        return Result.Ok(rights);
    }
}