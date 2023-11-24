using System.Net;
using Dumbogram.Common.Errors;

namespace Dumbogram.Core.Chats.Errors;

public class ChatAlreadyPublicError : ApplicationApiError
{
    public ChatAlreadyPublicError()
        : base(nameof(ChatAlreadyPublicError), HttpStatusCode.Conflict)
    {
    }
}

public class ChatAlreadyPrivateError : ApplicationApiError
{
    public ChatAlreadyPrivateError()
        : base(nameof(ChatAlreadyPrivateError), HttpStatusCode.Conflict)
    {
    }
}