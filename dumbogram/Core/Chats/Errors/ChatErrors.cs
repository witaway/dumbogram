using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class ChatNotFoundError : BaseApplicationError
{
    public ChatNotFoundError(string message)
        : base(nameof(ChatNotFoundError), message)
    {
    }
}