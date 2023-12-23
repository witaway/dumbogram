using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;

namespace Dumbogram.Api.Api.Messages.Responses;

public class SingleMessageResponse
{
    public SingleMessageResponse(Message message, bool isReplyInner = false)
    {
        MessageId = message.Id;
        SenderId = message.SenderId;
        ChatId = message.ChatId;

        if (message is UserMessage userMessage)
        {
            Content = userMessage.Content;
            if (userMessage.RepliedMessage is not null && !isReplyInner)
                ReplyToMessage = new SingleMessageResponse(
                    userMessage.RepliedMessage,
                    true
                );
        }
        else if (message is SystemMessage systemMessage)
        {
            SystemMessage = systemMessage.SystemMessageType.ToString();
            SystemMessageDetails = systemMessage.SystemMessageDetails ?? new SystemMessageDetails();
        }
    }

    public int MessageId { get; set; }
    public Guid? SenderId { get; set; }
    public Guid ChatId { get; set; }
    public object? Content { get; set; }
    public SingleMessageResponse? ReplyToMessage { get; set; }
    public string? SystemMessage { get; set; }
    public object? SystemMessageDetails { get; set; }
}