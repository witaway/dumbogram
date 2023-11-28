using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Chats.Errors;

public class ChatNotFoundError : ApplicationApiError
{
    public ChatNotFoundError()
        : base(nameof(ChatNotFoundError), HttpStatusCode.NotFound)
    {
    }
}