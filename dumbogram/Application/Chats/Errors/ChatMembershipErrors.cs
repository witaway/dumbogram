using System.Net;
using Dumbogram.Common.Errors;

namespace Dumbogram.Application.Chats.Errors;

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