using Dumbogram.Api.Models.Chats;

namespace Dumbogram.Api.Application.Chats.Controllers.Dto;

public class ReadSingleChatShortInfoResponse
{
    public ReadSingleChatShortInfoResponse(Chat chat)
    {
        ChatId = chat.Id;
        Title = chat.Title;
        Description = chat.Description;
    }

    public Guid ChatId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}