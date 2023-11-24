using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class ChatNotFoundError : ApplicationError
{
    public ChatNotFoundError()
        : base(nameof(ChatNotFoundError))
    {
    }
}