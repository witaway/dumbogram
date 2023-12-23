using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Application.Errors.Chats;

public class ChatNotFoundError : ApplicationApiError
{
    public ChatNotFoundError()
        : base(nameof(ChatNotFoundError), HttpStatusCode.NotFound)
    {
    }
}