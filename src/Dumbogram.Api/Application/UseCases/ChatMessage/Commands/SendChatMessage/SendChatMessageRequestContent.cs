namespace Dumbogram.Api.Application.UseCases.ChatMessage.Commands.SendChatMessage;

public class SendChatMessageRequestContent
{
    public string? Text { get; set; }
    public Guid? AttachedPhotosGroupId { get; set; }
}