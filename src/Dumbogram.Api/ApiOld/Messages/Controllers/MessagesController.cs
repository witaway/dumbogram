using Dumbogram.Api.ApiOld.Chats.Services;
using Dumbogram.Api.ApiOld.Messages.Controllers.Dto;
using Dumbogram.Api.ApiOld.Messages.Services;
using Dumbogram.Api.ApiOld.Users.Services;
using Dumbogram.Api.Infrasctructure.Controller;
using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.ApiOld.Messages.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}/messages", Name = "Messages")]
[ApiController]
public class MessagesController : ApplicationController
{
    private readonly ChatService _chatService;
    private readonly MessageContentBuilderService _messageContentBuilderService;
    private readonly MessagesService _messagesService;
    private readonly UserResolverService _userResolverService;

    public MessagesController(
        UserResolverService userResolverService,
        MessagesService messagesService,
        ChatService chatService,
        MessageContentBuilderService messageContentBuilderService
    )
    {
        _userResolverService = userResolverService;
        _messagesService = messagesService;
        _chatService = chatService;
        _messageContentBuilderService = messageContentBuilderService;
    }

    [HttpGet("{messageId:Guid}", Name = nameof(ReadSingleMessage))]
    public async Task<IActionResult> ReadSingleMessage(Guid chatId, int messageId)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed) return Failure(chatResult.Errors);

        var chat = chatResult.Value;

        var messageResult = await _messagesService.QuerySingleMessageById(subjectUser, chat, messageId);
        if (messageResult.IsFailed) return Failure(messageResult.Errors);

        var message = messageResult.Value;
        var messageResponse = new ReadSingleMessageResponse(message);

        return Ok(messageResponse);
    }

    [HttpGet(Name = nameof(ReadMultipleMessages))]
    public async Task<IActionResult> ReadMultipleMessages(Guid chatId)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed) return Failure(chatResult.Errors);

        var chat = chatResult.Value;

        var messagesResult = await _messagesService.QueryManyMessages(subjectUser, chat);
        if (messagesResult.IsFailed) return Failure(messagesResult.Errors);

        var messages = messagesResult.Value;
        var messagesResponse = new ReadManyMessagesResponse(messages);

        return Ok(messagesResponse);
    }

    [HttpPost(Name = nameof(SendSingleMessage))]
    public async Task<IActionResult> SendSingleMessage(Guid chatId, [FromBody] MessageContentRequest request)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed) return Failure(chatResult.Errors);

        var chat = chatResult.Value;

        var messageContentResult = await _messageContentBuilderService.BuildMessageContent(subjectUser, request);

        if (messageContentResult.IsFailed) return Failure(messageContentResult.Errors);

        var message = new UserMessage
        {
            Chat = chat,
            SenderProfile = subjectUser,
            Content = messageContentResult.Value
            // RepliedMessageId = request.ReplyTo
        };

        var sendMessageResult = await _messagesService.SendMessage(subjectUser, message);

        if (sendMessageResult.IsFailed) return Failure(sendMessageResult.Errors);

        return Ok();
    }

    [HttpPatch("{messageId:int}", Name = nameof(UpdateSingleMessage))]
    public async Task<IActionResult> UpdateSingleMessage(Guid chatId)
    {
        // Todo: Implement later
        throw new NotImplementedException();
    }

    [HttpDelete("{messageId:int}", Name = nameof(DeleteSingleMessage))]
    public async Task<IActionResult> DeleteSingleMessage(Guid chatId, int messageId)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed) return Failure(chatResult.Errors);

        var chat = chatResult.Value;

        var messageResult = await _messagesService.QuerySingleMessageById(subjectUser, chat, messageId);
        if (messageResult.IsFailed) return Failure(messageResult.Errors);

        var message = messageResult.Value;
        var deleteMessageResult = await _messagesService.DeleteMessage(subjectUser, message);
        if (deleteMessageResult.IsFailed) return Failure(deleteMessageResult.Errors);

        return Ok();
    }
}