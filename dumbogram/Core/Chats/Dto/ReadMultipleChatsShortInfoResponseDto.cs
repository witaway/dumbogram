using Dumbogram.Core.Chats.Models;

namespace Dumbogram.Core.Chats.Dto;

public class ReadMultipleChatsShortInfoResponseDto : List<ReadSingleChatShortInfoResponseDto>
{
    public ReadMultipleChatsShortInfoResponseDto(IEnumerable<Chat> chats)
    {
        AddRange(chats.Select(
            chat => new ReadSingleChatShortInfoResponseDto(chat)
        ));
    }
}