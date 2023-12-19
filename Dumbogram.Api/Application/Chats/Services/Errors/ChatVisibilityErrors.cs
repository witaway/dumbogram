using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.Application.Chats.Services.Errors;

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