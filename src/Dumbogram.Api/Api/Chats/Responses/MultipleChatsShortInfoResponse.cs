using Dumbogram.Api.Persistence.Context.Application.Entities.Chats;

namespace Dumbogram.Api.Api.Chats.Responses;

public class MultipleChatsShortInfoResponse : List<SingleChatShortInfoResponse>
{
    public MultipleChatsShortInfoResponse(IEnumerable<Chat> chats)
    {
        AddRange(chats.Select(
            chat => new SingleChatShortInfoResponse(chat)
        ));
    }
}