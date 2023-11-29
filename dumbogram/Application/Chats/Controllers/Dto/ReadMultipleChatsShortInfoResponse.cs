using Dumbogram.Models.Chats;

namespace Dumbogram.Application.Chats.Controllers.Dto;

public class ReadMultipleChatsShortInfoResponse : List<ReadSingleChatShortInfoResponse>
{
    public ReadMultipleChatsShortInfoResponse(IEnumerable<Chat> chats)
    {
        AddRange(chats.Select(
            chat => new ReadSingleChatShortInfoResponse(chat)
        ));
    }
}