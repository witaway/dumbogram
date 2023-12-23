using Dumbogram.Api.Persistence.Context.Application.Entities.Messages;

namespace Dumbogram.Api.Api.Messages.Responses;

public class MultipleMessagesResponse : List<SingleMessageResponse>
{
    public MultipleMessagesResponse(IEnumerable<Message> messages)
    {
        AddRange(messages.Select(message => new SingleMessageResponse(message)));
    }
}