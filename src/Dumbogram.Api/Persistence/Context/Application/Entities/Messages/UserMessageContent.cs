namespace Dumbogram.Api.Persistence.Context.Application.Entities.Messages;

public class UserMessageContent
{
    public string? Text { get; set; }
    public Guid? AttachedPhotosGroupId { get; set; }
}