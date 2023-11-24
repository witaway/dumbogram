using Dumbogram.Application.Chats.Models;

namespace Dumbogram.Application.Chats.Dto;

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