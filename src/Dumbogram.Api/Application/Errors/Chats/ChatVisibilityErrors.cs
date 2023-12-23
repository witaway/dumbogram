using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Application.Errors.Chats;

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