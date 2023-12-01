using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Chats.Services.Errors;
using Dumbogram.Models.Chats;
using Dumbogram.Models.Messages;
using Dumbogram.Models.Users;
using FluentResults;

namespace Dumbogram.Application.Messages.Services;

public class MessageActionsGuardService
{
    private readonly ChatMembershipService _chatMembershipService;
    private readonly ChatPermissionsService _chatPermissionsService;

    public MessageActionsGuardService(
        ChatMembershipService chatMembershipService,
        ChatPermissionsService chatPermissionsService
    )
    {
        _chatMembershipService = chatMembershipService;
        _chatPermissionsService = chatPermissionsService;
    }

    public async Task<Result> CheckMessagesCanBeRead(UserProfile subjectUser, Chat chat)
    {
        var isMember = await _chatMembershipService.IsUserJoinedToChat(
            subjectUser,
            chat
        );

        if (!isMember)
        {
            return Result.Fail(new UserNotInChatError());
        }

        return Result.Ok();
    }

    private async Task<Result> CheckSendAbility(UserProfile subjectUser, UserMessage message)
    {
        var senderUser = message.SubjectProfile;
        var destinationChat = message.Chat;

        var isMember = await _chatMembershipService.IsUserJoinedToChat(
            subjectUser,
            destinationChat
        );

        if (!isMember)
        {
            return Result.Fail(new UserNotInChatError());
        }

        var isSubjectAndSenderSame = senderUser == subjectUser;

        if (!isSubjectAndSenderSame)
        {
            return Result.Fail(new NotEnoughRightsError());
        }

        var isCanWrite = await _chatPermissionsService.IsUserHasRightInChat(
            destinationChat,
            senderUser,
            MembershipRight.Write
        );

        if (!isCanWrite)
        {
            return Result.Fail(new NotEnoughRightsError());
        }

        return Result.Ok();
    }

    public async Task<Result> CheckMessageCanBeSend(UserProfile subjectUser, UserMessage message)
    {
        var errors = new List<IError>();

        var sendAbilityResult = await CheckSendAbility(subjectUser, message);
        errors.AddRange(sendAbilityResult.Errors);

        return errors.Any()
            ? Result.Fail(errors)
            : Result.Ok();
    }

    public async Task<Result> CheckMessageCanBeDeletedBy(UserProfile subjectUser, Message message)
    {
        var chat = message.Chat;

        var isMemberOfChat = await _chatMembershipService.IsUserJoinedToChat(
            subjectUser,
            chat
        );

        if (!isMemberOfChat)
        {
            return Result.Fail(new UserNotInChatError());
        }

        var isSenderOfMessage = subjectUser == message.SubjectProfile;
        var isSenderOwnerOfChat = subjectUser == message.Chat.OwnerProfile;

        if (!isSenderOfMessage && !isSenderOwnerOfChat)
        {
            return Result.Fail(new NotEnoughRightsError());
        }

        return Result.Ok();
    }

    public async Task<Result> CheckMessageCanBeUpdatedBy(UserProfile subjectUser, UserMessage message)
    {
        var chat = message.Chat;

        var isMemberOfChat = await _chatMembershipService.IsUserJoinedToChat(
            subjectUser,
            chat
        );

        if (!isMemberOfChat)
        {
            return Result.Fail(new UserNotInChatError());
        }

        var isSenderOfMessage = subjectUser == message.SubjectProfile;

        if (!isSenderOfMessage)
        {
            return Result.Fail(new NotEnoughRightsError());
        }

        return Result.Ok();
    }
}