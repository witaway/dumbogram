using Dumbogram.Application.Chats.Services;
using Dumbogram.Application.Messages.Controllers.Dto;
using Dumbogram.Application.Messages.Services;
using Dumbogram.Application.Users.Services;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Models.Messages.UserMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Messages.Controllers;

[Authorize]
[Route("/api/chats/{chatId:guid}/messages", Name = "Messages")]
[ApiController]
public class MessagesController : ApplicationController
{
    private readonly ChatService _chatService;
    private readonly MessagesService _messagesService;
    private readonly UserResolverService _userResolverService;

    public MessagesController(
        UserResolverService userResolverService,
        MessagesService messagesService,
        ChatService chatService
    )
    {
        _userResolverService = userResolverService;
        _messagesService = messagesService;
        _chatService = chatService;
    }

    [HttpGet("{messageId:int}", Name = nameof(ReadSingleMessage))]
    public async Task<IActionResult> ReadSingleMessage(Guid chatId, int messageId)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;

        var messageResult = await _messagesService.QuerySingleMessageById(subjectUser, chat, messageId);
        if (messageResult.IsFailed)
        {
            return Failure(messageResult.Errors);
        }

        var message = messageResult.Value;
        var messageResponse = new ReadSingleMessageResponse(message);

        return Ok(messageResponse);
    }

    [HttpGet(Name = nameof(ReadMultipleMessages))]
    public async Task<IActionResult> ReadMultipleMessages(Guid chatId)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;

        var messagesResult = await _messagesService.QueryManyMessages(subjectUser, chat);
        if (messagesResult.IsFailed)
        {
            return Failure(messagesResult.Errors);
        }

        var messages = messagesResult.Value;
        var messagesResponse = new ReadManyMessagesResponse(messages);

        return Ok(messagesResponse);
    }

    [HttpPost(Name = nameof(SendSingleMessage))]
    public async Task<IActionResult> SendSingleMessage(Guid chatId, [FromBody] SendSingleMessageRequest request)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var chatResult = await _chatService.RequestPublicOrAccessibleChatByChatId(chatId, subjectUser);
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;

        var message = new RegularUserMessage
        {
            Chat = chat,
            SubjectProfile = subjectUser,
            Content = request.Content!,
            RepliedMessageId = request.ReplyTo
        };

        var sendMessageResult = await _messagesService.SendMessage(subjectUser, message);

        if (sendMessageResult.IsFailed)
        {
            return Failure(sendMessageResult.Errors);
        }

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
        if (chatResult.IsFailed)
        {
            return Failure(chatResult.Errors);
        }

        var chat = chatResult.Value;

        var messageResult = await _messagesService.QuerySingleMessageById(subjectUser, chat, messageId);
        if (messageResult.IsFailed)
        {
            return Failure(messageResult.Errors);
        }

        var message = messageResult.Value;
        var deleteMessageResult = await _messagesService.DeleteMessage(subjectUser, message);
        if (deleteMessageResult.IsFailed)
        {
            return Failure(deleteMessageResult.Errors);
        }

        return Ok();
    }
}