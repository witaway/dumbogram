using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Application.Errors.Messages;

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