using System.Net;
using Dumbogram.Common.Errors;

namespace Dumbogram.Application.Chats.Errors;

public class ChatNotFoundError : ApplicationApiError
{
    public ChatNotFoundError()
        : base(nameof(ChatNotFoundError), HttpStatusCode.NotFound)
    {
    }
}