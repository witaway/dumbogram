using Dumbogram.Models.Chats;

namespace Dumbogram.Application.Chats.Controllers.Dto;

public class ReadSingleChatShortInfoResponse
{
    public ReadSingleChatShortInfoResponse(Chat chat)
    {
        Title = chat.Title;
        Description = chat.Description;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
}