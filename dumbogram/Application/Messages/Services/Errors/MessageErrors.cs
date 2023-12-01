using System.Net;
using Dumbogram.Infrasctructure.Errors;

namespace Dumbogram.Application.Messages.Services.Errors;

public class MessageNotFoundError : ApplicationApiError
{
    public MessageNotFoundError()
        : base(nameof(MessageNotFoundError), HttpStatusCode.NotFound)
    {
    }
}