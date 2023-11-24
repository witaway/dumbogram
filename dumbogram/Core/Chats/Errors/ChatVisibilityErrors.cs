using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class ChatAlreadyPublicError : ApplicationError
{
    public ChatAlreadyPublicError()
        : base(nameof(ChatAlreadyPublicError))
    {
    }
}

public class ChatAlreadyPrivateError : ApplicationError
{
    public ChatAlreadyPrivateError()
        : base(nameof(ChatAlreadyPrivateError))
    {
    }
}