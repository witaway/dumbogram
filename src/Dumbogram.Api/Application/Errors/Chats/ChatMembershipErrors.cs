using System.Net;
using Dumbogram.Api.Common.Errors;

namespace Dumbogram.Api.Application.Errors.Chats;

public class UserAlreadyJoinedToChatError : ApplicationApiError
{
    public UserAlreadyJoinedToChatError()
        : base(nameof(UserAlreadyJoinedToChatError), HttpStatusCode.Conflict)
    {
    }
}

public class UserAlreadyBannedInChatError : ApplicationApiError
{
    public UserAlreadyBannedInChatError()
        : base(nameof(UserAlreadyBannedInChatError), HttpStatusCode.Conflict)
    {
    }
}

public class UserAlreadyLeftFromChatError : ApplicationApiError
{
    public UserAlreadyLeftFromChatError()
        : base(nameof(UserAlreadyLeftFromChatError), HttpStatusCode.Conflict)
    {
    }
}

public class UserBannedInChatError : ApplicationApiError
{
    public UserBannedInChatError()
        : base(nameof(UserBannedInChatError), HttpStatusCode.Forbidden)
    {
    }
}

public class UserNotInChatError : ApplicationApiError
{
    public UserNotInChatError()
        : base(nameof(UserNotInChatError), HttpStatusCode.Forbidden)
    {
    }
}