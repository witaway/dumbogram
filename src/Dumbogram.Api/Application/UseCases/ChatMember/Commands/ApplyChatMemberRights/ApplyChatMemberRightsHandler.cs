using Dumbogram.Api.Application.Errors.Chats;
using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Users;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMember.Commands.ApplyChatMemberRights;

public class ApplyChatMemberRightsHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    ChatPermissionsService chatPermissionsService,
    UserService userService
) : IRequestHandler<ApplyChatMemberRightsRequest, Result>
{
    public async Task<Result> Handle(
        ApplyChatMemberRightsRequest request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();

        // Retrieve chat
        var chatId = request.ChatId;
        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(chatId, currentUser);
        if (chatResult.IsFailed) return Result.Fail(chatResult.Errors);

        // Check permissions
        var chat = chatResult.Value;
        var isOwner = chatPermissionsService.IsUserOwnerOfChat(chat, currentUser);
        if (!isOwner) return Result.Fail(new NotEnoughRightsError());

        // Retrieve needed user
        var memberId = request.MemberId;
        var memberProfileResult = await userService.RequestUserProfileById(memberId);
        if (memberProfileResult.IsFailed) return Result.Fail(memberProfileResult.Errors);
        var memberProfile = memberProfileResult.Value;

        // Restrict permission changes if user is owner itself
        var isMemberOwner = chatPermissionsService.IsUserOwnerOfChat(chat, memberProfile);
        if (isMemberOwner) return Result.Fail(new CannotChangeOwnerRights());

        // Change rights
        var rights = request.Rights;
        await chatPermissionsService.EnsureRightsAppliedToUserInChat(chat, memberProfile, rights);

        return Result.Ok();
    }
}