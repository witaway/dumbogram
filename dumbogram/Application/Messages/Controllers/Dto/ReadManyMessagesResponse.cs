using Dumbogram.Models.Messages;

namespace Dumbogram.Application.Messages.Controllers.Dto;

public class ReadManyMessagesResponse : List<ReadSingleMessageResponse>
{
    public ReadManyMessagesResponse(IEnumerable<Message> messages)
    {
        AddRange(messages.Select(message => new ReadSingleMessageResponse(message)));
    }
}