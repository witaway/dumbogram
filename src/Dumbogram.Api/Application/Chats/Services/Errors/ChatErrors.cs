using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.Application.Chats.Services.Errors;

public class ChatNotFoundError : ApplicationApiError
{
    public ChatNotFoundError()
        : base(nameof(ChatNotFoundError), HttpStatusCode.NotFound)
    {
    }
}