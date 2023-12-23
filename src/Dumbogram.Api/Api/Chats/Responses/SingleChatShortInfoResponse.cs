using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;

namespace Dumbogram.Api.Api.Chats.Responses;

public class SingleChatShortInfoResponse
{
    public SingleChatShortInfoResponse(Chat chat)
    {
        ChatId = chat.Id;
        Title = chat.Title;
        Description = chat.Description;
    }

    public Guid ChatId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}