using Dumbogram.Core.Chats.Models;

namespace Dumbogram.Core.Chats.Dto;

public class ReadChatByChatIdResponseDto
{
    public required string Title { get; set; }
    public required string? Description { get; set; }

    public static ReadChatByChatIdResponseDto MapFromModel(Chat chat)
    {
        return new ReadChatByChatIdResponseDto
        {
            Title = chat.Title,
            Description = chat.Description
        };
    }
}