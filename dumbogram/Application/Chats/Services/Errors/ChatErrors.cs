using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Chats.Services.Errors;

public class ChatNotFoundError : ApplicationApiError
{
    public ChatNotFoundError()
        : base(nameof(ChatNotFoundError), HttpStatusCode.NotFound)
    {
    }
}