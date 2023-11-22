using Dumbogram.Core.Chats.Models;

namespace Dumbogram.Core.Chats.Dto;

public class ReadSingleChatShortInfoResponseDto
{
    public ReadSingleChatShortInfoResponseDto(Chat chat)
    {
        Title = chat.Title;
        Description = chat.Description;
    }

    public string Title { get; set; }
    public string? Description { get; set; }
}