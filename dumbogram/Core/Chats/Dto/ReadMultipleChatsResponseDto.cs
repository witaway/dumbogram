using Dumbogram.Core.Chats.Models;

namespace Dumbogram.Core.Chats.Dto;

public class ReadMultipleChatsResponseDto : List<ReadSingleChatByChatIdResponseDto>
{
    public ReadMultipleChatsResponseDto(IEnumerable<Chat> chats)
    {
        AddRange(chats.Select(chat => new ReadSingleChatByChatIdResponseDto(chat)));
    }
}