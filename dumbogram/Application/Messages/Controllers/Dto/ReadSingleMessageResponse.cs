﻿using Dumbogram.Models.Messages;
using Dumbogram.Models.Messages.SystemMessages;
using Dumbogram.Models.Messages.UserMessages;

namespace Dumbogram.Application.Messages.Controllers.Dto;

public class ReadSingleMessageResponse
{
    public ReadSingleMessageResponse(Message message, bool isReplyInner = false)
    {
        SenderId = message.SubjectId;
        ChatId = message.ChatId;

        if (message is RegularUserMessage regularUserMessage)
        {
            Content = regularUserMessage.Content;
            if (regularUserMessage.RepliedMessage is not null && !isReplyInner)
            {
                ReplyToMessage = new ReadSingleMessageResponse(
                    regularUserMessage.RepliedMessage,
                    true
                );
            }
        }

        if (message is SystemMessage systemMessage)
        {
            SystemMessage = systemMessage.GetType().ToString();
            if (systemMessage is EditedTitleSystemMessage editedTitleSystemMessage)
            {
                SystemMessageDetails = new
                {
                    editedTitleSystemMessage.NewTitle
                };
            }

            if (systemMessage is EditedDescriptionSystemMessage editedDescriptionSystemMessage)
            {
                SystemMessageDetails = new
                {
                    editedDescriptionSystemMessage.NewDescription
                };
            }
        }
    }

    public Guid SenderId { get; set; }
    public Guid ChatId { get; set; }
    public string? Content { get; set; }
    public ReadSingleMessageResponse? ReplyToMessage { get; set; }
    public string? SystemMessage { get; set; }
    public object? SystemMessageDetails { get; set; }
}