using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;

namespace Dumbogram.Api.ApiOld.Chats.Controllers.Dto;

public class ReadMultipleChatsShortInfoResponse : List<ReadSingleChatShortInfoResponse>
{
    public ReadMultipleChatsShortInfoResponse(IEnumerable<Chat> chats)
    {
        AddRange(chats.Select(
            chat => new ReadSingleChatShortInfoResponse(chat)
        ));
    }
}