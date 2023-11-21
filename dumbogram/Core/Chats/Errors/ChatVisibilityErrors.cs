using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class ChatAlreadyPublic : BaseApplicationError
{
    public ChatAlreadyPublic(string message)
        : base(nameof(ChatAlreadyPublic), message)
    {
    }
}

public class ChatAlreadyPrivate : BaseApplicationError
{
    public ChatAlreadyPrivate(string message)
        : base(nameof(ChatAlreadyPrivate), message)
    {
    }
}