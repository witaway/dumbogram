using Dumbogram.Api.Application.Errors.Messages;
using Dumbogram.Api.Common.Extensions;
using Dumbogram.Api.Domain.Services.Chats;
using Dumbogram.Api.Domain.Services.Messages;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Infrastructure.Files;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.ChatMessage.Commands.SendChatMessage;

public class SendChatMessageHandler(
    UserResolverService userResolverService,
    ChatService chatService,
    MessagesService messagesService,
    FilesGroupService filesGroupService
) : IRequestHandler<SendChatMessageRequest, Result<UserMessage>>
{
    public async Task<Result<UserMessage>> Handle(SendChatMessageRequest request, CancellationToken cancellationToken)
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var chatId = request.ChatId;

        // Retrieve chat
        var chatResult = await chatService.RequestPublicOrAccessibleChatByChatId(chatId, currentUser);
        if (chatResult.IsFailed) return Result.Fail(chatResult.Errors);
        var chat = chatResult.Value;

        // Build and validate content of message
        var messageContentResult = await BuildMessageContent(currentUser, request.MessageContent);
        if (messageContentResult.IsFailed) return Result.Fail(messageContentResult.Errors);

        // Create message
        var message = new UserMessage
        {
            Chat = chat,
            SenderProfile = currentUser,
            Content = messageContentResult.Value
        };

        var sendMessageResult = await messagesService.SendMessage(currentUser, message);

        if (sendMessageResult.IsFailed) return Result.Fail(sendMessageResult.Errors);

        return Result.Ok(message);
    }

    private async Task<Result<UserMessageContent>> BuildMessageContent(
        UserProfile sender,
        SendChatMessageRequestContent messageContentRequest
    )
    {
        var content = new UserMessageContent();

        var results = new List<Result>
        {
            SetText(),
            await SetAttachedPhotos(),
            CheckHasNotNullFields()
        };

        var errors = results.GetErrors();

        return errors.Any()
            ? Result.Fail(errors)
            : Result.Ok(content);

        Result SetText()
        {
            var text = messageContentRequest.Text;
            if (text is null) return Result.Ok();

            var trimmedText = text.Trim();
            if (trimmedText.Length == 0) return Result.Ok();

            content!.Text = trimmedText;
            return Result.Ok();
        }

        async Task<Result> SetAttachedPhotos()
        {
            var groupId = messageContentRequest.AttachedPhotosGroupId;
            if (groupId is null) return Result.Ok();

            var groupResult = await filesGroupService.RequestOwnedFilesGroupById(
                sender,
                (Guid)groupId
            );

            // Check if current user owns group
            if (groupResult.IsFailed) return Result.Fail(groupResult.Errors);

            // Check if group is of correct type
            var group = groupResult.Value;
            if (group.GroupType != FilesGroupType.AttachedPhotos)
                return Result.Fail(new BadMessageContent());

            content!.AttachedPhotosGroupId = groupId;
            return Result.Ok();
        }

        Result CheckHasNotNullFields()
        {
            if (content.AttachedPhotosGroupId is not null) return Result.Ok();
            if (content.Text is not null) return Result.Ok();

            return Result.Fail(new MessageCannotBeEmpty());
        }
    }
}