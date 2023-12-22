using System.Net;
using Dumbogram.Api.Infrasctructure.Errors;

namespace Dumbogram.Api.ApiOld.Messages.Services.Errors;

public class MessageNotFoundError : ApplicationApiError
{
    public MessageNotFoundError()
        : base(nameof(MessageNotFoundError), HttpStatusCode.NotFound)
    {
    }
}

public class MessageCannotBeEmpty : ApplicationApiError
{
    public MessageCannotBeEmpty()
        : base(nameof(MessageCannotBeEmpty), HttpStatusCode.BadRequest)
    {
    }
}

public class BadMessageContent : ApplicationApiError
{
    public BadMessageContent()
        : base(nameof(BadMessageContent), HttpStatusCode.BadRequest)
    {
    }
}