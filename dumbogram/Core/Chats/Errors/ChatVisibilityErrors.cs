using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class ChatAlreadyPublicError : BaseApplicationError
{
    public ChatAlreadyPublicError(string message)
        : base(nameof(ChatAlreadyPublicError), message)
    {
    }
}

public class ChatAlreadyPrivateError : BaseApplicationError
{
    public ChatAlreadyPrivateError(string message)
        : base(nameof(ChatAlreadyPrivateError), message)
    {
    }
}